using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwiftUserManagement.Application.Features.Commands.AnalyseGameResults
{
    public class AnalyseGameResultsCommand : IRequest<string>
    {
        public string result1 { get; set; }
        public string result2 { get; set; }
    }
}
