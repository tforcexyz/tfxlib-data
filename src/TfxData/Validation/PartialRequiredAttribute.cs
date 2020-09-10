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

  public class PartialRequiredAttribute : ValidationAttribute
  {

    public PartialRequiredAttribute(params string[] otherProperties)
        : base(Formats.Properties_0_Required)
    {
      OtherProperties = otherProperties;
      OtherPropertyDisplayNames = new string[otherProperties.Length];
    }

    public bool AllowEmptyString { get; set; }

    protected string[] OtherProperties { get; }

    protected string[] OtherPropertyDisplayNames { get; }

    public override bool RequiresValidationContext
    {
      get
      {
        return true;
      }
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
      object[] otherPropertyValues = new object[OtherProperties.Length];
      for (int i = 0; i < OtherProperties.Length; i++)
      {
        string otherPropertyName = OtherProperties[i];
        PropertyInfo otherPropertyInfo = validationContext.ObjectType.GetProperty(otherPropertyName);
        if (otherPropertyInfo == null)
        {
          return new ValidationResult(string.Format(CultureInfo.CurrentCulture, Formats.Property_0_Unknown, otherPropertyName));
        }

        if (OtherPropertyDisplayNames[i] == null)
        {
          OtherPropertyDisplayNames[i] = GetDisplayNameForProperty(validationContext.ObjectType, otherPropertyName) ?? otherPropertyName;
        }
        otherPropertyValues[i] = otherPropertyInfo.GetValue(validationContext.ObjectInstance, null);
      }
      foreach (object otherValue in otherPropertyValues)
      {
        if (!Equals(value, otherValue))
        {
          List<string> displayNames = new List<string>
          {
            validationContext.DisplayName
          };
          displayNames.AddRange(OtherPropertyDisplayNames);
          string concatenatedName = string.Join(", ", displayNames);
          return new ValidationResult(string.Format(ErrorMessageString, concatenatedName));
        }
      }
      return null;
    }

    private static string GetDisplayNameForProperty(Type containerType, string propertyName)
    {
      string displayName = propertyName;
      PropertyInfo propertyInfo = containerType.GetProperty(propertyName);
      DisplayAttribute displayAttribute = propertyInfo.GetCustomAttribute<DisplayAttribute>();
      if (displayAttribute != null)
      {
        displayName = displayAttribute.GetName();
        return displayName;
      }
      DisplayNameAttribute displayNameAttribute = propertyInfo.GetCustomAttribute<DisplayNameAttribute>();
      if (displayNameAttribute != null)
      {
        displayName = displayNameAttribute.DisplayName;
      }
      return displayName;
    }
  }
}
