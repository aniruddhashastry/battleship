using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BattleshipStateTracker.Api
{
    public class BattleshipGameSettings
    {
        public int BoardSize { get; set; }
        public string AwsCognitoAppClientId { get; set; }
        public string AwsCognitoUserPoolId { get; set; }
    }
}
