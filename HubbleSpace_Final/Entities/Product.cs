﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HubbleSpace_Final.Entities
{
    [Table("Product")]
    public class Product
    {
        [Key]
        public int ID_Product { get; set; }

        [Display(Name = "Tên")]
        [Required(ErrorMessage = "Không được để trống")]
        [MaxLength(100, ErrorMessage = "Không hợp lệ")]
        public string Product_Name { get; set; }

        [Display(Name = "Giá niêm yết")]
        [Required(ErrorMessage = "Không được để trống")]
        [Range(0, 10000000000, ErrorMessage = "Không hợp lệ")]
        public double Price_Product { get; set; }

        [Range(0, 10000000000, ErrorMessage = "Không hợp lệ")]
        [Display(Name = "Giá bán")]
        public double Price_Sale { get; set; }

        [Display(Name = "Thương hiệu")]
        public int ID_Brand { get; set; }
        [ForeignKey("ID_Brand")]
        public Brand Brand { get; set; }

        [Display(Name = "Danh mục")]
        public int ID_Categorie { get; set; }
        [ForeignKey("ID_Categorie")]
        public Category category { get; set; }

        [Display(Name = "Mô tả")]
        public string Description { get; set; }

        public ICollection<Color_Product> Color_Products { get; set; }
        public Product()
        {
            Color_Products = new HashSet<Color_Product>();
        }

    }

}
