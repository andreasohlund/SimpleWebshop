﻿namespace Sales.Api.Controllers;

using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Sales.Api.Data;
    
[Route("product")]
public class ProductPriceController(SalesDbContext context) : Controller
{
    [HttpGet("{id}", Name = "GetProduct")]
    public IActionResult GetById(long id)
    {
        var item = context.ProductPrices.FirstOrDefault(t => t.Id == id);
        if (item == null)
        {
            return NotFound();
        }

        return new ObjectResult(item);
    }

    [HttpGet]
    public IActionResult GetById(string productIds)
    {
        if (productIds == null)
        {
            return NotFound();
        }

        var productIdList = productIds.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)
            .ToList();
        var productsList = context.ProductPrices.Where(p => productIdList.Contains(p.ProductId)).ToList();
        return new ObjectResult(productsList);
    }
}