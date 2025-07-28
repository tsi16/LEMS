﻿using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace LEMS.ViewModels;
public class LoginViewModel
{
    [Required(ErrorMessage = "User Name is required")]
    public string UserName { get; set; }

    [Required(ErrorMessage = "Password is required")]
    [DataType(DataType.Password)]
    public string Password { get; set; }
}
