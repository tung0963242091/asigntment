using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using T1808A_Asigntment.Entity;

namespace T1808A_Asigntment.Service
{
    interface IMemberService
    {
        Member Register(Member member);

        MemberCredential Login(MemberLogin memberLogin);

        Member GetInformation(string token);
    }
}
