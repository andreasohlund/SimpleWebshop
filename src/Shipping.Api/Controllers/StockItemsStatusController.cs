﻿namespace Shipping.Api.Controllers;

using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Shipping.Api.Data;

[Route("product")]
public class StockItemsStatusController(StockItemDbContext context) : Controller
{
    [HttpGet("{id}", Name = "GetProduct")]
    public IActionResult GetById(string id)
    {
        var item = context.StockItems.FirstOrDefault(t => t.ProductId == id);
        if (item == null)
        {
            return NotFound();
        }

        return new ObjectResult(item);
    }

    [HttpGet]
    public IActionResult GetByIds(string productIds)
    {
        if (productIds == null)
        {
            return NotFound();
        }

        var productIdList = productIds.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)
            .ToList();
        var productsList = context.StockItems.Where(p => productIdList.Contains(p.ProductId)).ToList();
        return new ObjectResult(productsList);
    }
}