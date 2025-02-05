using Tes.Data;
using Tes.Domain;

namespace Tes.Business
{
    public class TesProcessor : TesDataAccessControl
    {
        #region View
        public async Task<List<MsUserResponse>> GetListUser()
        {
            try
            {
                var result = await GetListUserAsync();
                return result;
            }
            catch
            {
                throw;
            }
        }
        public async Task<List<ProductResponse>> GetListProduct()
        {
            try
            {
                var result = await GetListProductAsync();
                return result;
            }
            catch
            {
                throw;
            }
        }
        #endregion View

        #region Transaction
        public async Task<TransactionResponse> SubmitUser(MsUserRequest data)
        {
            try
            {
                var result = await SubmitUserAsync(data);
                return result;
            }
            catch
            {
                throw;
            }
        }
        public async Task<TransactionResponse> SubmitProduct(ProductRequest data)
        {
            try
            {
                var result = await SubmitProductAsync(data);
                return result;
            }
            catch
            {
                throw;
            }
        }
        #endregion Transaction
    }
}
