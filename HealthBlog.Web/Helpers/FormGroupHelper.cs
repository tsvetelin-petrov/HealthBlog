namespace HealthBlog.Web.Helpers
{
	using Microsoft.AspNetCore.Html;
	using Microsoft.AspNetCore.Mvc.Rendering;
	using System;
	using System.IO;
	using System.Linq.Expressions;
	using System.Text.Encodings.Web;

	public static class FormGroupHelper
	{
		//public static IHtmlContent FormGroupSubmit(this IHtmlHelper htmlHelper,  string backAction, string submitValue = "Create", string backController = "", string additioanlAttributes = "")
		//{
		//	using (var writer = new StringWriter())
		//	{
		//		writer.Write($@"
		//			<div class=""mt-3"">
		//				<input type=""submit"" class=""btn btn-success"" value=""{submitValue}"" />
		//				<a asp-action=""{backAction}"" asp-controller=""{backController}"" asp-area="""" class=""btn btn-light"">Back</a>
		//			</div>");

		//		return new HtmlString(writer.ToString());
		//	}
		//}

		public static IHtmlContent FormGroupFor<TModel, TResult>(
			this IHtmlHelper<TModel> htmlHelper,
			Expression<Func<TModel, TResult>> expression)
		{
			/*<div class="input-group mt-4">
				<div class="input-group-prepend">
					<label asp-for="@Model.Input.RepetitionCount" class="input-group-text"></label>
				</div>
				<input asp-for="@Model.Input.RepetitionCount" class="form-control" />
				</div>
				<span asp-validation-for="@Model.Input.RepetitionCount" class="text-danger"></span>
			*/

			using (var writer = new StringWriter())
			{
				var lable = htmlHelper.LabelFor(expression, new { @class = "input-group-text" });
				var editor = htmlHelper.EditorFor(expression, new { htmlAttributes = new { @class = "form-control" } });
				var validationMessage = htmlHelper.ValidationMessageFor(expression, null, new { @class = "text-danger" });

				writer.Write("<div class=\"input-group mt-4\"><div class=\"input-group-prepend\">");
				lable.WriteTo(writer, HtmlEncoder.Default);
				writer.Write("</div>");
				editor.WriteTo(writer, HtmlEncoder.Default);
				writer.Write("</div>");
				validationMessage.WriteTo(writer, HtmlEncoder.Default);
				return new HtmlString(writer.ToString());
			}
		}
	}
}
