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

namespace Xyz.TForce.Data
{

  public class ColumnLengths
  {

    public const int NormalLength = 256;
    public const int MediumLength = 1024;
    public const int LongLength = 2048;

    public const int IdLength = 128;
    public const int KeyLength = 256;
    public const int NameLength = 512;
    public const int HashLength = 256;
    public const int EmailLength = 256;
    public const int ValueLength = 1024;
    public const int UnicodeValueLength = 2048;
    public const int AggregatedIdsLength = 2048;
    public const int SummaryLength = 4000;
    public const int FreeTextLength = int.MaxValue;
  }
}
