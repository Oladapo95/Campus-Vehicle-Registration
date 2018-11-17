[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(VechicleRegistration.MVCGridConfig), "RegisterGrids")]

namespace VechicleRegistration
{
    using System;
    using System.Web;
    using System.Web.Mvc;
    using System.Linq;
    using System.Collections.Generic;

    using MVCGrid.Models;
    using MVCGrid.Web;
    using Models;
    using Context;
    using Models.Repositories;

    public static class MVCGridConfig
    {

        public static void RegisterGrids()
        {
            var vrcon = new VRContext();
            MVCGridDefinitionTable.Add("SGrid", new MVCGridBuilder<Vehicle>()
    .WithAuthorizationType(AuthorizationType.AllowAnonymous)
    .AddColumns(cols =>
    { 
        cols.Add("StudentID").WithHeaderText("REG ID")
            .WithValueExpression(p => p.StudentID.ToString());
        cols.Add("MatricNumber").WithHeaderText("Matric Number")
            .WithValueExpression(p => p.Student.MatricNumber.ToString());
        cols.Add("Department").WithHeaderText("Department")
            .WithValueExpression(p => p.Student.Department ?? "CSE");
        cols.Add("PlateNumber").WithHeaderText("Plate Number")
            .WithValueExpression(p => p.Plate_number.ToString());
        cols.Add("Make").WithHeaderText("Vehicle Make")
            .WithValueExpression(p => p.Make ?? "Toyota Corolla");
        cols.Add("Color").WithHeaderText("Vehicle Color")
            .WithValueExpression(p => p.Color ?? "Red");
        cols.Add("Edit").WithHtmlEncoding(false).WithHeaderText("Actions")
            .WithSorting(false)
            .WithHeaderText(" ")
            .WithValueExpression((p, c) => c.UrlHelper.Action("detail", "demo", new { id = p.StudentID }))
            .WithValueTemplate("<a href='{Value}' class='btn btn-primary' role='button'>Edit</a>");
        cols.Add("Delete").WithHtmlEncoding(false)
            .WithSorting(false)
            .WithHeaderText(" ")
            .WithValueExpression((p, c) => c.UrlHelper.Action("Details", "Admin", new { id = p.StudentID }))
            .WithValueTemplate("<a href='{Value}' class='btn btn-danger' role='button'>Delete</a>");
        cols.Add("View").WithHtmlEncoding(false)
            .WithSorting(false)
            .WithHeaderText(" ")
            .WithValueExpression((p, c) => c.UrlHelper.Action("DetailsPage", "Admin", new { id = p.StudentID }))
            .WithValueTemplate("<button type='button' class='btn btn-warning' onClick='viewRecord({Model.StudentID})' data-toggle='modal' data-target='#myModal'>View</button>");
    })
    .WithSorting(true, "MatricNumber")
    .WithPaging(true, 10)
    .WithAdditionalQueryOptionNames("Search")
    .WithAdditionalSetting("RenderLoadingDiv", false)
    .WithRetrieveDataMethod((options) =>
    {
        //var options = context.QueryOptions;
        //int totalRecords;
        //var repo = DependencyResolver.Current.GetService<IPersonRepository>();
        //string globalSearch = options.GetAdditionalQueryOptionString("Search");
        //var items = repo.GetData(out totalRecords, globalSearch, options.GetLimitOffset(), options.GetLimitRowcount(),
        //    options.SortColumnName, options.SortDirection == SortDirection.Dsc);
        //return new QueryResult<Vehicle>()
        //{
        //    Items = items,
        //    TotalRecords = totalRecords
        //};


        //var result = new QueryResult<Student>();
        //var result2 = new QueryResult<Vehicle>();
        //string globalSearch = options.GetAdditionalQueryOptionString("Search");

        //using (var db = new VRContext())
        //{
        //    //db.Vehicles.Any(m => m.Color == "rr");
        //    //result.Items = db.Students.ToList();
        //    ////result.Items = db.Students.Where(p => p.StudentID == 2).ToList();
        //    //result2.Items = db.Vehicles.Include("Student").ToList();
        //    //return result2;


        //    var con = options.QueryOptions; 
        //    var queryResult = new QueryResult<Vehicle>();

            
        //    return queryResult;
        //}

        using (var dbf = new VRContext()) 
        {
            var con = options.QueryOptions;
            var queryResult = new QueryResult<Vehicle>();

            var context = options.QueryOptions;
            string globalSearch = con.GetAdditionalQueryOptionString("Search");
            var query = dbf.Vehicles.Include("Student").AsQueryable();
            var repo = DependencyResolver.Current.GetService<StudentRepository>();
            queryResult.TotalRecords = query.Count();
            if (!String.IsNullOrWhiteSpace(con.SortColumnName))
            {
                switch (con.SortColumnName.ToLower())
                {
                    case "matricnumber":
                        if (con.SortDirection == SortDirection.Asc)
                            query = query.OrderBy(c => c.Student.MatricNumber);
                        else if (con.SortDirection == SortDirection.Dsc)
                            query = query.OrderByDescending(c => c.Student.MatricNumber);
                        break;
                    case "department":
                        if (con.SortDirection == SortDirection.Asc)
                            query = query.OrderBy(c => c.Student.Department);
                        else if (con.SortDirection == SortDirection.Dsc)
                            query = query.OrderByDescending(c => c.Student.Department);
                        break;
                }
            }
            if (con.GetLimitOffset().HasValue)
            {
                query = query.OrderBy(i => i.Student.MatricNumber).Skip(con.GetLimitOffset().Value).Take(con.GetLimitRowcount().Value);

            }

            if (!String.IsNullOrWhiteSpace(globalSearch))
            {
                //query = query.OrderBy(i => i.Student.MatricNumber).Skip(con.GetLimitOffset().Value).Take(con.GetLimitRowcount().Value);
                query = query.Where(i => i.Student.Department.Contains(globalSearch));
            }

            queryResult.Items = query.ToList();

            return queryResult;
        }
    })
);

        }
    }
}