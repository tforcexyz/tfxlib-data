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

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Xyz.TForce.Data.Validation
{

  public class ValidationExpress
  {

    public static ValidatedResult Validate(object model)
    {
      ValidatedResult result = new ValidatedResult();
      if (model == null)
      {
        result.Validations.AddItem(typeof(ValidationExpress).FullName, new string[] { Messages.ModelNull }, new string[] { nameof(model) });
        return result;
      }
      ValidationContext validationContext = new ValidationContext(model);
      ICollection<ValidationResult> validationResults = new List<ValidationResult>();
      if (!Validator.TryValidateObject(model, validationContext, validationResults))
      {
        foreach (ValidationResult validationResult in validationResults)
        {
          result.Validations.AddItem(validationResult.GetType().FullName, new string[] { validationResult.ErrorMessage }, validationResult.MemberNames);
        }
      }
      return result;
    }
  }
}
