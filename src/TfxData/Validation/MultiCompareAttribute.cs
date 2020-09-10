// Copyright 2017 T-Force Xyz
// Please refer to LICENSE & CONTRIB files in the project root for license information.
//
// Licensed under the Apache License, Version 2.0 (the "License"),
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//   http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Reflection;

namespace Xyz.TForce.Data.Validation
{

  public class MultiCompareAttribute : ValidationAttribute
  {

    private readonly Type _stringType;

    public MultiCompareAttribute(params string[] otherProperties)
        : base(Formats.Properties_0_EqualsRequired)
    {
      OtherProperties = otherProperties;
      OtherPropertyDisplayNames = new string[otherProperties.Length];
      _stringType = typeof(string);
    }

    protected string[] OtherProperties { get; }

    protected string[] OtherPropertyDisplayNames { get; }

    public bool AllowEmptyString { get; set; }

    public override bool RequiresValidationContext
    {
      get
      {
        return true;
      }
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
      ValidationResult validationResult = null;
      object[] otherPropertyValues = new object[OtherProperties.Length];
      for (int i = 0; i < OtherProperties.Length; i++)
      {
        string otherProperty = OtherProperties[i];
        PropertyInfo otherPropertyInfo = validationContext.ObjectType.GetProperty(otherProperty);
        if (otherPropertyInfo == null)
        {
          return new ValidationResult(string.Format(CultureInfo.CurrentCulture, Formats.Property_0_Unknown, otherProperty));
        }

        if (OtherPropertyDisplayNames[i] == null)
        {
          OtherPropertyDisplayNames[i] = GetDisplayNameForProperty(validationContext.ObjectType, otherProperty) ?? otherProperty;
        }
        otherPropertyValues[i] = otherPropertyInfo.GetValue(validationContext.ObjectInstance, null);
      }

      List<string> displayNames = new List<string>
      {
          validationContext.DisplayName
      };
      displayNames.AddRange(OtherPropertyDisplayNames);
      string concatenatedName = string.Join(", ", displayNames);
      int emptyCount = 0;
      if (IsEmpty(value))
      {
        emptyCount++;
      }
      foreach (object otherValue in otherPropertyValues)
      {
        if (IsEmpty(otherValue))
        {
          emptyCount++;
        }
      }
      if (emptyCount == OtherProperties.Length + 1)
      {
        validationResult = new ValidationResult(string.Format(ErrorMessageString, concatenatedName));
      }
      return validationResult;
    }

    private static string GetDisplayNameForProperty(Type containerType, string propertyName)
    {
      string displayName = propertyName;
      PropertyInfo propertyInfo = containerType.GetProperty(propertyName);
      if (propertyInfo == null)
      {
        throw new NullReferenceException(nameof(propertyInfo));
      }
      DisplayAttribute displayAttribute = propertyInfo.GetCustomAttribute<DisplayAttribute>();
      if (displayAttribute != null)
      {
        propertyName = displayAttribute.GetName();
        return propertyName;
      }
      DisplayNameAttribute displayNameProperty = propertyInfo.GetCustomAttribute<DisplayNameAttribute>();
      if (displayNameProperty != null)
      {
        displayName = displayNameProperty.DisplayName;
      }
      return displayName;
    }

    private bool IsEmpty(object value)
    {
      if (value == null)
      {
        return true;
      }
      if (!AllowEmptyString)
      {
        if (value.GetType() == _stringType)
        {
          if (string.IsNullOrWhiteSpace(value.ToString()))
          {
            return true;
          }
        }
      }
      return false;
    }
  }
}
