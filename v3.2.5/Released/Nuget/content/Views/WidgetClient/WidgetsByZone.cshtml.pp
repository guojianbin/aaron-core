﻿@model List<RenderWidgetModel>
@using $rootnamespace$.Models.Cms;
@foreach (var widget in Model)
{
    @Html.Action(widget.ActionName, widget.ControllerName, widget.RouteValues)
}