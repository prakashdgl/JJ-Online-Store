﻿using JjOnlineStore.Common.ViewModels.Products;
using JjOnlineStore.Services.Core;
using JjOnlineStore.Common.ViewModels;

using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Mvc;

using System.Threading.Tasks;
using JjOnlineStore.Common.Enumeration;

using static JjOnlineStore.Common.GlobalConstants;
using static JjOnlineStore.Web.ViewPaths;

namespace JjOnlineStore.Web.Controllers
{
    [Route("[controller]/[action]")]
    public class ProductsController : BaseController
    {
        private readonly IProductsService _productsService;
        private readonly IUserViewedItemsService _userViewedItemsService;

        public ProductsController(
            IProductsService productsService, 
            IUserViewedItemsService userViewedItemsService)
        {
            _productsService = productsService;
            _userViewedItemsService = userViewedItemsService;
        }

        /// GET: /Products/Index
        /// <summary>
        /// Base products view with model: collection of products.
        /// </summary>
        public async Task<IActionResult> Index(string errorMsg = null)
        {
            if (errorMsg != null)
            {
                TempData[ErrorMessage] = errorMsg;
            }
            return View(await _productsService.AllWithoutDeletedAsync());
        }

        /// GET: /Products/GetByMainCategory
        /// <summary>
        /// Returns collection of Products by Category -> Main Store Category.
        /// It is visualized in the Products->Index view.
        /// </summary>
        /// <param name="category">Main Store Category</param>
        public async Task<IActionResult> GetByMainCategory(MainStoreCategories category) =>
            View(ProductsIndex, await _productsService.GetByMainCategoryAsync(category));

        /// GET: /Products/SearchByCategoryAndWord
        /// <summary>
        /// Returns collection of Products by Category -> Main Store Category
        /// and searched word.
        /// It is visualized in the Products->Index view. 
        /// </summary>
        /// <param name="category">Main Store Category</param>
        /// <param name="searchedWord"></param>
        public async Task<IActionResult> SearchByCategoryAndWord([FromQuery]
            MainStoreCategories category,
            string searchedWord) =>
            View(ProductsIndex, await _productsService.GetByMainCategoryAndWordAsync(category,searchedWord));


        /// GET: /Products/Details?Id
        /// <summary>
        /// Adds current product to 'UserViewedItems'.
        /// Gets product details.
        /// </summary>
        /// <param name="id">Product ID.</param>
        /// <returns>Option of ProductViewModel or Error.</returns>
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            await _userViewedItemsService.AddAsync(User.Identity.GetUserId(), id.Value);

            return (await _productsService.GetByIdAsync(id.Value))
                .Match(DisplayDetails, DetailsError);
        }

        /// <summary>
        /// Displays product details.
        /// </summary>
        /// <param name="model">Valid Product Model.</param>
        private IActionResult DisplayDetails(ProductViewModel model)
            => View(ProductsDetails, model);

        /// <summary>
        /// Redirects to /Products/Index 
        /// and displays product details error in fancy-box.
        /// </summary>
        private IActionResult DetailsError(Error error)
            => RedirectToAction(nameof(Index), new { errorMsg = error.ToString() });
    }
}