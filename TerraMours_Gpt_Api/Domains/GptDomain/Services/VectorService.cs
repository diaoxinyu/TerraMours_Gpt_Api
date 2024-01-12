using AllInAI.Sharp.API.Dto;
using AllInAI.Sharp.API.Req;
using AllInAI.Sharp.API.Res.Vector;
using AutoMapper;
using IdentityModel;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Ocsp;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using System.Xml.Linq;
using TerraMours.Domains.LoginDomain.Contracts.Common;
using TerraMours.Framework.Infrastructure.EFCore;
using TerraMours_Gpt_Api.Domains.GptDomain.Contracts.Res;
using TerraMours_Gpt_Api.Domains.GptDomain.IServices;

namespace TerraMours_Gpt_Api.Domains.GptDomain.Services {
    public class VectorService : IVectorService
    {
        private readonly FrameworkDbContext _dbContext;
        private readonly Serilog.ILogger _logger;
        private readonly IMapper _mapper;

        public VectorService(FrameworkDbContext dbContext, Serilog.ILogger logger, IMapper mapper)
        {
            _dbContext = dbContext;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<ApiResponse<bool>> CreateIndex(string name, int knowledgeId)
        {
            var know =await _dbContext.knowledgeItems.FirstOrDefaultAsync(m => m.KnowledgeId == knowledgeId);
            if (know == null)
                return ApiResponse<bool>.Fail("未找到对应知识库记录");
            var type = (AllInAI.Sharp.API.Enums.AITypeEnum)know.KnowledgeType;
            string baseUrl = type== AllInAI.Sharp.API.Enums.AITypeEnum.Pinecone? $"https://controller.{know.NamespaceName}.pinecone.io":know.BaseUrl;
            AuthOption authOption = new AuthOption() { Key = know.ApiKey, BaseUrl = baseUrl, AIType = type };
            AllInAI.Sharp.API.Service.VectorService vectorService = new AllInAI.Sharp.API.Service.VectorService(authOption);
            await vectorService.CreateIndex(name, 1536, Metric.Cosine);
            return ApiResponse<bool>.Success(true);
        }

        public async Task<ApiResponse<bool>> Delete(int knowledgeId, VectorDeleteReq req)
        {
            var know = await _dbContext.knowledgeItems.FirstOrDefaultAsync(m => m.KnowledgeId == knowledgeId);
            if (know == null)
                return ApiResponse<bool>.Fail("未找到对应知识库记录");
            var type = (AllInAI.Sharp.API.Enums.AITypeEnum)know.KnowledgeType;
            string baseUrl = type == AllInAI.Sharp.API.Enums.AITypeEnum.Pinecone ? $"https://{know.IndexName}.{know.NamespaceName}.pinecone.io" : $"{know.BaseUrl}/{know.IndexName}/";
            AuthOption authOption = new AuthOption() { Key = know.ApiKey, BaseUrl = baseUrl, AIType = type };
            AllInAI.Sharp.API.Service.VectorService vectorService = new AllInAI.Sharp.API.Service.VectorService(authOption);
            await vectorService.Delete(req);
            return ApiResponse<bool>.Success(true);
        }

        public async Task<ApiResponse<bool>> DeleteIndex(string name, int knowledgeId)
        {
            var know = await _dbContext.knowledgeItems.FirstOrDefaultAsync(m => m.KnowledgeId == knowledgeId);
            if (know == null)
                return ApiResponse<bool>.Fail("未找到对应知识库记录");
            var type = (AllInAI.Sharp.API.Enums.AITypeEnum)know.KnowledgeType;
            string baseUrl = type == AllInAI.Sharp.API.Enums.AITypeEnum.Pinecone ? $"https://controller.{know.NamespaceName}.pinecone.io" : know.BaseUrl;
            AuthOption authOption = new AuthOption() { Key = know.ApiKey, BaseUrl = baseUrl, AIType = type };
            AllInAI.Sharp.API.Service.VectorService vectorService = new AllInAI.Sharp.API.Service.VectorService(authOption);
            await vectorService.DeleteIndex(name);
            return ApiResponse<bool>.Success(true);
        }

        public async Task<ApiResponse<IndexStats>> DescribeIndexStats(int knowledgeId)
        {
            var know = await _dbContext.knowledgeItems.FirstOrDefaultAsync(m => m.KnowledgeId == knowledgeId);
            if (know == null)
                return ApiResponse<IndexStats>.Fail("未找到对应知识库记录");
            var type = (AllInAI.Sharp.API.Enums.AITypeEnum)know.KnowledgeType;
            string baseUrl = type == AllInAI.Sharp.API.Enums.AITypeEnum.Pinecone ? $"https://controller.{know.NamespaceName}.pinecone.io" : know.BaseUrl;
            AuthOption authOption = new AuthOption() { Key = know.ApiKey, BaseUrl = baseUrl, AIType = type };
            AllInAI.Sharp.API.Service.VectorService vectorService = new AllInAI.Sharp.API.Service.VectorService(authOption);
            var res= await vectorService.DescribeIndexStats();
            return ApiResponse<IndexStats>.Success(res);
        }

        public Task<ApiResponse<List<VectorQueryRes>>> GetList(int knowledgeId)
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResponse<List<string>>> ListIndexes(int knowledgeId)
        {
            var know = await _dbContext.knowledgeItems.FirstOrDefaultAsync(m => m.KnowledgeId == knowledgeId);
            if (know == null)
                return ApiResponse<List<string>>.Fail("未找到对应知识库记录");
            var type = (AllInAI.Sharp.API.Enums.AITypeEnum)know.KnowledgeType;
            string baseUrl = type == AllInAI.Sharp.API.Enums.AITypeEnum.Pinecone ? $"https://controller.{know.NamespaceName}.pinecone.io" : know.BaseUrl;
            AuthOption authOption = new AuthOption() { Key = know.ApiKey, BaseUrl = baseUrl, AIType = type };
            AllInAI.Sharp.API.Service.VectorService vectorService = new AllInAI.Sharp.API.Service.VectorService(authOption);
            var res = await vectorService.ListIndexes();
            return ApiResponse<List<string>>.Success(res);
        }

        public async Task<ApiResponse<VectorQueryRes>> Query(VectorQueryReq req, int knowledgeId)
        {
            var know = await _dbContext.knowledgeItems.FirstOrDefaultAsync(m => m.KnowledgeId == knowledgeId);
            if (know == null)
                return ApiResponse<VectorQueryRes>.Fail("未找到对应知识库记录");
            var type = (AllInAI.Sharp.API.Enums.AITypeEnum)know.KnowledgeType;
            string baseUrl = type == AllInAI.Sharp.API.Enums.AITypeEnum.Pinecone ? $"https://{know.IndexName}.{know.NamespaceName}.pinecone.io" : $"{know.BaseUrl}/{know.IndexName}/";
            AuthOption authOption = new AuthOption() { Key = know.ApiKey, BaseUrl = baseUrl, AIType = type };
            AllInAI.Sharp.API.Service.VectorService vectorService = new AllInAI.Sharp.API.Service.VectorService(authOption);
            string json = JsonSerializer.Serialize(req, new JsonSerializerOptions()
            {
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
            });
            var res = await vectorService.Query(req);
            return ApiResponse<VectorQueryRes>.Success(res);
        }

        public async Task<ApiResponse<bool>> Update(VectorUpdateReq req, int knowledgeId)
        {
            var know = await _dbContext.knowledgeItems.FirstOrDefaultAsync(m => m.KnowledgeId == knowledgeId);
            if (know == null)
                return ApiResponse<bool>.Fail("未找到对应知识库记录");
            var type = (AllInAI.Sharp.API.Enums.AITypeEnum)know.KnowledgeType;
            string baseUrl = type == AllInAI.Sharp.API.Enums.AITypeEnum.Pinecone ? $"https://{know.IndexName}.{know.NamespaceName}.pinecone.io" : $"{know.BaseUrl}/{know.IndexName}/";
            AuthOption authOption = new AuthOption() { Key = know.ApiKey, BaseUrl = baseUrl, AIType = type };
            AllInAI.Sharp.API.Service.VectorService vectorService = new AllInAI.Sharp.API.Service.VectorService(authOption);
            await vectorService.Update(req);
            return ApiResponse<bool>.Success(true);
        }

        public async Task<ApiResponse<VectorUpsertRes>> Upsert(VectorUpsertReq req, int knowledgeId)
        {
            var know = await _dbContext.knowledgeItems.FirstOrDefaultAsync(m => m.KnowledgeId == knowledgeId);
            if (know == null)
                return ApiResponse<VectorUpsertRes>.Fail("未找到对应知识库记录");
            var type = (AllInAI.Sharp.API.Enums.AITypeEnum)know.KnowledgeType;
            string baseUrl = type == AllInAI.Sharp.API.Enums.AITypeEnum.Pinecone ? $"https://{know.IndexName}.{know.NamespaceName}.pinecone.io" : $"{know.BaseUrl}/{know.IndexName}/";
            AuthOption authOption = new AuthOption() { Key = know.ApiKey, BaseUrl = baseUrl, AIType = type };
            AllInAI.Sharp.API.Service.VectorService vectorService = new AllInAI.Sharp.API.Service.VectorService(authOption);
            var res = await vectorService.Upsert(req);
            return ApiResponse<VectorUpsertRes>.Success(res);
        }
    }
}
