
using System;
using Application.Enums;

namespace Application.Common.Models.BaseResult
{
	public class Error
	{
		public string Message { get; set; }
		public ExceptionType Code { get; set; }
	}
}
