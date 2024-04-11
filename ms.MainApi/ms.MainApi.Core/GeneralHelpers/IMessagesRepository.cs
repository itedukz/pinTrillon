﻿using Microsoft.Extensions.Configuration;

namespace ms.MainApi.Core.GeneralHelpers;

public interface IMessagesRepository
{
    string AccessDenied(string data);
    string WrongDataFormat(string data);
    string FormValidation();
    string Deleted();
    string Created(string data);
    string Edited(string data);
    string NotEmpty(string data);
    string ShouldBeUnique(string data);
    string ValidValueInEnum(string data, string enumName);
    string NotFound();
    string NotFound(string data);
    string NotEqual(string data, string equalData);
    string CantDelete(string reason);
}

public class MessagesRepository : IMessagesRepository
{
    private readonly IConfiguration _configuration;

    public MessagesRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string AccessDenied(string data) =>
        _configuration.GetSection("ResponseMessages").GetSection("AccessDenied").Value!.Replace("{{NAME}}", data);
    
    public string WrongDataFormat(string data) =>
        _configuration.GetSection("ResponseMessages").GetSection("WrongDataFormat").Value!.Replace("{{PROPERTYNAME}}", data);

    public string FormValidation() =>
        _configuration.GetSection("ResponseMessages").GetSection("FormValidation").Value!;

    public string Deleted() =>
        _configuration.GetSection("ResponseMessages").GetSection("Deleted").Value!;

    public string Created(string data) =>
        _configuration.GetSection("ResponseMessages").GetSection("Created").Value!.Replace("{{NAME}}", data);

    public string Edited(string data) =>
        _configuration.GetSection("ResponseMessages").GetSection("Edited").Value!.Replace("{{NAME}}", data);

    public string NotEmpty(string data) =>
        _configuration.GetSection("ResponseMessages").GetSection("NotEmpty").Value!.Replace("{{PROPERTYNAME}}", data);

    public string ShouldBeUnique(string data) =>
        _configuration.GetSection("ResponseMessages").GetSection("ShouldBeUnique").Value!.Replace("{{PROPERTYNAME}}",
            data);

    public string ValidValueInEnum(string data, string enumName) =>
        _configuration.GetSection("ResponseMessages").GetSection("ValidValueInEnum").Value!
            .Replace("{{PROPERTYNAME}}", data).Replace("{{ENUMNAME}}", enumName);

    public string NotFound() => _configuration.GetSection("ResponseMessages").GetSection("NotFound").Value!;

    public string NotEqual(string data, string equalData) =>
        _configuration.GetSection("ResponseMessages").GetSection("NotEqual").Value!
            .Replace("{{PROPERTYNAME}}", data).Replace("{{EQUALDATA}}", equalData);

    public string CantDelete(string reason) =>
        _configuration.GetSection("ResponseMessages").GetSection("CantDelete").Value!
            .Replace("{{REASON}}", reason);
    public string NotFound(string data) =>
        _configuration.GetSection("ResponseMessages").GetSection("NotFoundWithMessage").Value!
            .Replace("{{PROPERTYNAME}}", data);
}