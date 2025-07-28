using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using OT.ServiceLayer.DTOs;
using OT.ServiceLayer.Interfaces;
using OT.PresentationLayer.ViewModels;

namespace OT.PresentationLayer.Controllers;

public class ProductController : Controller
{
    private readonly IProductService _productService;
    private readonly IMapper _mapper;

    public ProductController(IProductService productService, IMapper mapper)
    {
        _productService = productService;
        _mapper = mapper;
    }

    public async Task<IActionResult> Index()
    {
        var products = await _productService.GetAllAsync();
        var viewModels = _mapper.Map<IEnumerable<ProductViewModel>>(products);
        return View(viewModels);
    }

    public async Task<IActionResult> Details(int id)
    {
        var product = await _productService.GetByIdAsync(id);
        if (product == null)
            return NotFound();

        var viewModel = _mapper.Map<ProductViewModel>(product);
        return View(viewModel);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ProductViewModel model)
    {
        if (ModelState.IsValid)
        {
            var dto = _mapper.Map<ProductDto>(model);
            await _productService.CreateAsync(dto);
            return RedirectToAction(nameof(Index));
        }
        return View(model);
    }

    public async Task<IActionResult> Edit(int id)
    {
        var product = await _productService.GetByIdAsync(id);
        if (product == null)
            return NotFound();

        var viewModel = _mapper.Map<ProductViewModel>(product);
        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, ProductViewModel model)
    {
        if (id != model.Id)
            return NotFound();

        if (ModelState.IsValid)
        {
            var dto = _mapper.Map<ProductDto>(model);
            await _productService.UpdateAsync(dto);
            return RedirectToAction(nameof(Index));
        }
        return View(model);
    }

    public async Task<IActionResult> Delete(int id)
    {
        var product = await _productService.GetByIdAsync(id);
        if (product == null)
            return NotFound();

        var viewModel = _mapper.Map<ProductViewModel>(product);
        return View(viewModel);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _productService.DeleteAsync(id);
        return RedirectToAction(nameof(Index));
    }
}