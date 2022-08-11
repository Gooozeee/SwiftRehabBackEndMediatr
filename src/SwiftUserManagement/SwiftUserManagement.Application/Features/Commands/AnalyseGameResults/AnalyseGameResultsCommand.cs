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
        public string UserName { get; set; }
        public int User_Id { get; set; }
        public int result1 { get; set; }
        public int result2 { get; set; }
        public int level { get; set; }
    }
}
