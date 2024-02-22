using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediatRDemo.RequestMsg
{
    public class MyRequest:IRequest<int>
    {
        public string RequestType { set; get; }
    }
}
