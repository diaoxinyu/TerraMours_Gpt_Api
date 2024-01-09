using TerraMours.Domains.LoginDomain.Contracts.Common;
using TerraMours_Gpt_Api.Domains.GptDomain.Contracts.Req;
using TerraMours_Gpt_Api.Domains.GptDomain.Contracts.Res;
using TerraMours_Gpt_Api.Domains.GptDomain.IServices;

namespace TerraMours_Gpt_Api.Domains.GptDomain.Services
{
    public class KnowledgeService : IKnowledgeService
    {
        public Task<ApiResponse<bool>> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse<List<KnowledgeRes>>> GetList(long userId)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse<List<KnowledgeRes>>> Query(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse<bool>> Update(KnowledgeUpdateReq req)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse<KnowledgeRes>> Upsert(KnowledgeReq req)
        {
            throw new NotImplementedException();
        }
    }
}
