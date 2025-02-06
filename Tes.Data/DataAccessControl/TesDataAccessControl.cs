using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PORECT.Helper;
using Tes.Data.Models;
using Tes.Domain;

namespace Tes.Data
{
    public class TesDataAccessControl
    {
        #region View
        public async Task<List<MsUserResponse>> GetListUserAsync(int? id = null, string? username = null)
        {
            using (var context = new DataContext())
            {
                if (context == null || context.MsUsers == null)
                    throw new ArgumentNullException("Db context not found");

                try
                {
                    var result = await context.MsUsers.Where(x =>
                        (id == null || x.Id == id) &&
                        (string.IsNullOrEmpty(username) || x.Username.ToLower() == username.ToLower().Trim()))
                    .Select(x => new MsUserResponse
                    {
                        ID = x.Id,
                        Username = x.Username,
                        Password = x.Password.Decrypt(),
                        FirstName = x.FirstName,
                        LastName = x.LastName,
                        IsActive = x.IsActive ?? true,
                        CreatedBy = x.CreatedBy,
                        CreatedDtm = x.CreatedDtm,
                        UpdatedBy = x.UpdatedBy,
                        UpdatedDtm = x.UpdatedDtm
                    }).ToListAsync();
                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }
        public async Task<List<ProductResponse>> GetListProductAsync(SearchProductRequest dto)
        {
            using (var context = new DataContext())
            {
                if (context == null || context.Products == null)
                    throw new ArgumentNullException("Db context not found");

                try
                {
                    var result = await context.Products.Where(x =>
                                (dto.ID == null || x.Id == dto.ID) &&
                                (string.IsNullOrEmpty(dto.Code) || x.ProductCode.Trim().ToLower() == dto.Code.Trim().ToLower()) &&
                                (string.IsNullOrEmpty(dto.Name) || x.Name.ToLower().Contains(dto.Name.Trim().ToLower())) &&
                                ((dto.PriceFrom == null && dto.PriceTo == null) ||
                                 (dto.PriceFrom != null && dto.PriceTo == null && dto.PriceFrom <= x.Price) ||
                                 (dto.PriceFrom == null && dto.PriceTo != null && x.Price <= dto.PriceTo) ||
                                 (dto.PriceFrom != null && dto.PriceTo != null &&
                                  dto.PriceFrom <= x.Price && x.Price <= dto.PriceTo)
                                ))
                    .Select(x => new ProductResponse
                    {
                        ID = x.Id,
                        ProductCode = x.ProductCode,
                        Name = x.Name,
                        Description = x.Description,
                        Price = x.Price,
                        CreatedBy = x.CreatedBy,
                        CreatedDtm = x.CreatedDtm,
                        UpdatedBy = x.UpdatedBy,
                        UpdatedDtm = x.UpdatedDtm
                    }).ToListAsync();
                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }
        #endregion View

        #region Transaction
        public async Task<TransactionResponse> SubmitUserAsync(MsUserRequest data)
        {
            using (var context = new DataContext())
            {
                if (context == null || context.MsUsers == null)
                    throw new Exception("Db context not found");

                var transaction = context.Database.BeginTransaction();
                try
                {
                    TransactionResponse result = new TransactionResponse();
                    if (string.IsNullOrEmpty(data.TransactionType) || (data.TransactionType.ToLower() != "insert" &&
                        data.TransactionType.ToLower() != "update" && data.TransactionType.ToLower() != "disable" &&
                        data.TransactionType.ToLower() != "enable"))
                    {
                        await transaction.RollbackAsync();
                        result.IsSuccess = false;
                        result.Message = "Invalid transaction type";
                        result.Data = JsonConvert.SerializeObject(new { Data = data });
                        return result;
                    }

                    if (data.TransactionType.ToLower() == "insert")
                    {
                        #region Check Existing
                        var qry = await context.MsUsers.FirstOrDefaultAsync(x => x.Username.ToLower().Trim() == data.Username.ToLower().Trim());
                        if (qry != null)
                        {
                            await transaction.RollbackAsync();
                            result.IsSuccess = false;
                            result.Message = "User already exist";
                            result.Data = JsonConvert.SerializeObject(new { Data = data });
                            return result;
                        }
                        #endregion Check Existing

                        MsUser dto = new MsUser
                        {
                            Username = data.Username.Trim(),
                            Password = data.Password.Encrypt(),
                            FirstName = data.FirstName.Trim(),
                            LastName = data.LastName?.Trim()//,
                            //IsActive = data.IsActive ?? true,
                            //CreatedBy = data.CreatedBy,
                            //CreatedDtm = data.CreatedDtm ?? DateTime.Now
                        };
                        context.Add(dto);
                    }
                    else
                    {
                        #region Get Data
                        var qry = await context.MsUsers.FirstOrDefaultAsync(x => x.Id == data.ID);
                        if (qry == null)
                        {
                            await transaction.RollbackAsync();
                            result.IsSuccess = false;
                            result.Message = "Data not found";
                            result.Data = JsonConvert.SerializeObject(new { Data = data });
                            return result;
                        }
                        #endregion Get Data

                        if (data.TransactionType.ToLower() == "update")
                        {
                            #region Check Existing
                            if (qry.Username.ToLower().Trim() != data.Username.ToLower().Trim())
                            {
                                var check = await context.MsUsers.FirstOrDefaultAsync(x => x.Username.ToLower().Trim() == data.Username.ToLower().Trim());
                                if (check != null)
                                {
                                    await transaction.RollbackAsync();
                                    result.IsSuccess = false;
                                    result.Message = "User already exist";
                                    result.Data = JsonConvert.SerializeObject(new { Data = data });
                                    return result;
                                }
                            }
                            #endregion Check Existing

                            qry.Password = !string.IsNullOrEmpty(data.Password?.Trim()) ? data.Password.Trim().Encrypt() :
                                                qry.Password;
                            qry.FirstName = !string.IsNullOrEmpty(data.FirstName?.Trim()) ? data.FirstName.Trim() :
                                                qry.FirstName;
                            qry.LastName = data.LastName?.Trim();
                            //qry.IsActive = data.IsActive ?? qry.IsActive;
                        }
                        else
                            qry.IsActive = data.TransactionType.ToLower() == "disable" ? false : true;

                        qry.UpdatedBy = data.CreatedBy;
                        qry.UpdatedDtm = DateTime.Now;
                        context.Entry(qry).State = EntityState.Detached;
                        context.Update(qry);
                    }
                    await context.SaveChangesAsync();

                    bool ready = true;
                    if (ready)
                    {
                        await transaction.CommitAsync();
                        result.Message = string.Format("User {0} {1} successfully.", 
                                            string.Format("{0}{1}", data.FirstName,
                                                !string.IsNullOrEmpty(data.LastName) ? 
                                                    string.Concat(" ", data.LastName) :
                                                    string.Empty
                                            ),
                                            data.TransactionType.ToLower() == "disable" ? "disabled" :
                                            data.TransactionType.ToLower() == "enable" ? "enabled" : "submitted");
                        //result.Data = JsonConvert.SerializeObject(data);
                    }
                    else
                    {
                        await transaction.RollbackAsync();
                        result.IsSuccess = false;
                        result.Message = string.Format("Fail to {0} user {1}.",
                                            data.TransactionType.ToLower() == "disable" ? "disable" :
                                            data.TransactionType.ToLower() == "enable" ? "enable" : "submit",
                                            string.Format("{0}{1}", data.FirstName,
                                                !string.IsNullOrEmpty(data.LastName) ?
                                                    string.Concat(" ", data.LastName) :
                                                    string.Empty
                                            ));
                        result.Data = JsonConvert.SerializeObject(new { Data = data });
                    }

                    return result;
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }
        public async Task<TransactionResponse> SubmitProductAsync(ProductRequest data)
        {
            using (var context = new DataContext())
            {
                if (context == null || context.Products == null)
                    throw new Exception("Db context not found");

                var transaction = context.Database.BeginTransaction();
                try
                {
                    TransactionResponse result = new TransactionResponse();
                    if (string.IsNullOrEmpty(data.TransactionType) || (data.TransactionType.ToLower() != "insert" &&
                        data.TransactionType.ToLower() != "update" && data.TransactionType.ToLower() != "delete"))
                    {
                        await transaction.RollbackAsync();
                        result.IsSuccess = false;
                        result.Message = "Invalid transaction type";
                        result.Data = JsonConvert.SerializeObject(new { Data = data });
                        return result;
                    }

                    if (data.TransactionType.ToLower() == "insert")
                    {
                        #region Check Existing
                        var qry = await context.Products.FirstOrDefaultAsync(x => 
                                    x.Name.ToLower().Trim() == data.Name.ToLower().Trim() &&
                                    x.ProductCode.ToLower().Trim() == data.ProductCode.ToLower().Trim()
                                    //(string.IsNullOrEmpty(x.Description.Trim()) && string.IsNullOrEmpty(data.Description.Trim())) ||
                                    //(!string.IsNullOrEmpty(x.Description.Trim()) && !string.IsNullOrEmpty(data.Description.Trim()) &&
                                    // x.Description.Trim().ToLower() == data.Description.Trim().ToLower())
                                  );
                        if (qry != null)
                        {
                            await transaction.RollbackAsync();
                            result.IsSuccess = false;
                            result.Message = string.Format("Product {0} with code {1} already exist", data.Name, data.ProductCode);
                            result.Data = JsonConvert.SerializeObject(new { Data = data });
                            return result;
                        }
                        #endregion Check Existing

                        Product dto = new Product
                        {
                            ProductCode = data.ProductCode.Trim().ToUpper(),
                            Name = data.Name.Trim(),
                            Description = data.Description?.Trim(),
                            Price = data.Price ?? 0,
                            CreatedBy = data.CreatedBy ?? "System"//,
                            //CreatedDtm = data.CreatedDtm ?? DateTime.Now
                        };
                        context.Add(dto);
                    }
                    else
                    {
                        #region Get Data
                        var qry = await context.Products.OrderByDescending(x => x.CreatedDtm)
                                        .FirstOrDefaultAsync(x => x.ProductCode.ToUpper() == data.ProductCode.ToUpper());
                        if (qry == null)
                        {
                            await transaction.RollbackAsync();
                            result.IsSuccess = false;
                            result.Message = "Data not found";
                            result.Data = JsonConvert.SerializeObject(new { Data = data });
                            return result;
                        }
                        #endregion Get Data

                        if (data.TransactionType.ToLower() == "update")
                        {
                            qry.Name = !string.IsNullOrEmpty(data.Name?.Trim()) ? data.Name.Trim() :
                                                qry.Name;
                            qry.Description = data.Description;
                            qry.Price = data.Price ?? qry.Price;
                            qry.UpdatedBy = data.CreatedBy;
                            qry.UpdatedDtm = DateTime.Now;

                            context.Entry(qry).State = EntityState.Detached;
                            context.Update(qry);
                        }
                        else
                            context.Remove(qry);
                    }
                    await context.SaveChangesAsync();

                    bool ready = true;
                    if (ready)
                    {
                        await transaction.CommitAsync();
                        result.Message = string.Format("Product {0} {1} successfully.", data.Name,
                                            data.TransactionType.ToLower() == "delete" ? "deleted" : "submitted");
                        //result.Data = JsonConvert.SerializeObject(data);
                    }
                    else
                    {
                        await transaction.RollbackAsync();
                        result.IsSuccess = false;
                        result.Message = string.Format("Fail to {0} product {1}.",
                                            data.TransactionType.ToLower() == "delete" ? "delete" : "submit",
                                            data.Name);
                        result.Data = JsonConvert.SerializeObject(new { Data = data });
                    }

                    return result;
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }
        #endregion Transaction

    }
}
