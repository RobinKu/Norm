/*
 * Norm
 * Copyright (C) 2014 Robin Kuijstermans
 * 
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program.  If not, see [http://www.gnu.org/licenses/].
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit;
using NUnit.Framework;
using Moq;
using Norm;
using NUnit.Framework.Constraints;
using System.Linq.Expressions;
using System.Data;

namespace Norm.Tests
{
    [TestFixture]
    public class BaseDataRecordToObjectConverterTests
    {
        [Test(Description=@"Tests if the TypeToCreate property returns the type of the typeparameter.")]
        public void TypeToCreateIsEqualToGenericParameter()
        {
            // Arrange
            Mock<BaseDataRecordToObjectConverter<int>> converter1Mock = new Mock<BaseDataRecordToObjectConverter<int>>();
            converter1Mock.CallBase = true;
            BaseDataRecordToObjectConverter<int> converter1 = converter1Mock.Object;

            Mock<BaseDataRecordToObjectConverter<string>> converter2Mock = new Mock<BaseDataRecordToObjectConverter<string>>();
            converter2Mock.CallBase = true;
            BaseDataRecordToObjectConverter<string> converter2 = converter2Mock.Object;

            // Act & assert
            Assert.That(converter1.TypeToCreate, new EqualConstraint(typeof(int)));
            Assert.That(converter2.TypeToCreate, new EqualConstraint(typeof(string)));
        }

        [Test(Description=@"Tests if the non-generic Convert method calls the generic Convert method.")]
        public void NonGenericConvertMethodCallsGeneric()
        {
            // Arrange
            IDataRecord record = new Mock<IDataRecord>().Object;

            Expression<Func<BaseDataRecordToObjectConverter<int>, int>> genericConvertMethod = c=>c.Convert(It.IsAny<IDataRecord>());

            Mock<BaseDataRecordToObjectConverter<int>> converterMock = new Mock<BaseDataRecordToObjectConverter<int>>();
            converterMock.CallBase = true;
            converterMock.Setup(genericConvertMethod).Verifiable();
            BaseDataRecordToObjectConverter<int> converter = converterMock.Object;
            IDataRecordToObjectConverter nonGenericConverter = (IDataRecordToObjectConverter)converter;

            // Act
            nonGenericConverter.Convert(record);

            // Assert
            converterMock.Verify(genericConvertMethod, Times.Once());
        }
    }
}
