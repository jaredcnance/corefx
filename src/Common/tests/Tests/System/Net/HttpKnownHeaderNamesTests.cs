﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;

using Xunit;

namespace Tests.System.Net
{
    public class HttpKnownHeaderNamesTests
    {
        [Theory]
        [InlineData("")]
        [InlineData("this should not be found")]
        public void TryGetHeaderName_UnknownStrings_NotFound(string shouldNotBeFound)
        {
            char[] key = shouldNotBeFound.ToCharArray();
            string name;

            Assert.False(HttpKnownHeaderNames.TryGetHeaderName(key, 0, key.Length, out name));
            Assert.Null(name);
        }

        [Theory]
        [MemberData("HttpKnownHeaderNamesPublicStringConstants")]
        public void TryGetHeaderName_AllHttpKnownHeaderNamesPublicStringConstants_Found(string constant)
        {
            char[] key = constant.ToCharArray();
            string name;

            Assert.True(HttpKnownHeaderNames.TryGetHeaderName(key, 0, key.Length, out name));
            Assert.NotNull(name);
            Assert.Equal(constant, name);
        }

        public static IEnumerable<object[]> HttpKnownHeaderNamesPublicStringConstants
        {
            get
            {
                string[] constants = typeof(HttpKnownHeaderNames)
                    .GetTypeInfo()
                    .DeclaredFields
                    .Where(f => f.IsStatic && f.IsPublic && f.FieldType == typeof(string))
                    .Select(f => (string)f.GetValue(null))
                    .ToArray();

                Assert.NotEmpty(constants);
                Assert.DoesNotContain(constants, c => string.IsNullOrEmpty(c));

                return constants.Select(c => new object[] { c });
            }
        }
    }
}
