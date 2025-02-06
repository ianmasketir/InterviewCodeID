using Tes.Data;
using Tes.Domain;

namespace Tes.Business
{
    public class UserProcessor : IUserRepository//TesDataAccessControl
    {
        private readonly TesDataAccessControl _access = new TesDataAccessControl();

        #region View
        public List<MsUserResponse> GetListUser(int? id = null, string? username = null)
        {
            try
            {
                var result = _access.GetListUserAsync(id, username).Result;
                //untuk case sederhana ini, abstraksi hanya ini karena semua operasi ada di repository

                return result;
            }
            catch
            {
                throw;
            }
        }
        #endregion View

        #region Transaction
        public TransactionResponse SubmitUser(MsUserRequest data)
        {
            try
            {
                var result = _access.SubmitUserAsync(data).Result;
                //untuk case sederhana ini, abstraksi hanya ini karena semua operasi ada di repository

                return result;
            }
            catch
            {
                throw;
            }
        }
        #endregion Transaction

    }
    public class ProductProcessor : IProductRepository//TesDataAccessControl
    {
        private readonly TesDataAccessControl _access = new TesDataAccessControl();

        #region View
        public List<ProductResponse> GetListProduct(SearchProductRequest dto)
        {
            try
            {
                var result = _access.GetListProductAsync(dto).Result;
                //untuk case sederhana ini, abstraksi hanya ini karena semua operasi ada di repository

                return result;
            }
            catch
            {
                throw;
            }
        }
        #endregion View

        #region Transaction
        public TransactionResponse SubmitProduct(ProductRequest data)
        {
            try
            {
                var result = _access.SubmitProductAsync(data).Result;
                //untuk case sederhana ini, abstraksi hanya ini karena semua operasi ada di repository

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
