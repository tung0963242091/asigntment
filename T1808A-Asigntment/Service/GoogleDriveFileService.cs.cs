using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using T1808A_Asigntment.Entity;

namespace T1808A_Asigntment.Service
{
    class GoogleDriveFileService : IFileService
    {
        public Task<bool> SaveMemberCredentialToFile(MemberCredential memberCredential)
        {
            throw new NotImplementedException();
        }

        public Task<MemberCredential> ReadMemberCredentialFromFile()
        {
            throw new NotImplementedException();
        }

        public void SignOutByDeleteTokenAsync()
        {
            throw new NotImplementedException();
        }
    }
}
