using TerraMours.Domains.LoginDomain.Contracts.Common;
using TerraMours_Gpt_Api.Domains.GptDomain.Contracts.Req;
using TerraMours_Gpt_Api.Domains.GptDomain.Contracts.Res;

namespace TerraMours_Gpt_Api.Domains.GptDomain.IServices
{
    public interface IKnowledgeService
    {
        Task<ApiResponse<List<KnowledgeRes>>> GetList(long userId);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<ApiResponse<bool>> Delete(int id);
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<ApiResponse<List<KnowledgeRes>>> Query(int id);
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<ApiResponse<bool>> Update(KnowledgeUpdateReq req);

        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<ApiResponse<KnowledgeRes>> Upsert(KnowledgeReq req);
    }
}
