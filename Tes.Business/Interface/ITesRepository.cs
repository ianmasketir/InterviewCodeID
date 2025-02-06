using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tes.Domain;

namespace Tes.Business
{
    public interface ITesRepository
    {
        #region View
        public List<MsUserResponse> GetListUser();
        public List<ProductResponse> GetListProduct(SearchProductRequest dto);
        #endregion View

        #region Transaction
        public TransactionResponse SubmitUser(MsUserRequest data);
        public TransactionResponse SubmitProduct(ProductRequest data);
        #endregion Transaction
    }
    public interface IUserRepository
    {
        #region View
        public List<MsUserResponse> GetListUser(int? id = null, string? username = null);
        #endregion View

        #region Transaction
        public TransactionResponse SubmitUser(MsUserRequest data);
        #endregion Transaction
    }
    public interface IProductRepository
    {
        #region View
        public List<ProductResponse> GetListProduct(SearchProductRequest dto);
        #endregion View

        #region Transaction
        public TransactionResponse SubmitProduct(ProductRequest data);
        #endregion Transaction
    }
}
