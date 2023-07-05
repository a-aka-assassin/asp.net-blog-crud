using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using aspnet_blog_application.Models;
using Microsoft.Data.Sqlite;
using aspnet_blog_application.Models.ViewModels;

namespace aspnet_blog_application.Controllers;

public class PostController : Controller
{
    private readonly ILogger<PostController> _logger;
    private readonly IConfiguration _configuration;

    public PostController(ILogger<PostController> logger, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
    }

    public IActionResult Index()
    {
        var PostList = GetAllPosts();
        return View(PostList);
    }

    public IActionResult NewPost()
    {
        return View();
    }

    public IActionResult ViewPost(int id)
    {
        var post = GetPostById(id);
        var postViewModel = new PostViewModel();
        postViewModel.Post = post;
        return View(postViewModel);
    }

    public IActionResult EditPost(int id)
    {
        var post = GetPostById(id);
        var postViewModel = new PostViewModel();
        postViewModel.Post = post;
        return View(postViewModel);
    }

    internal PostViewModel GetAllPosts()
    {
        List<PostModel> PostList = new();

        using (SqliteConnection connection = new SqliteConnection(_configuration.GetConnectionString("BlogDataContext")))
        {
            using ( var command = connection.CreateCommand() )
            {
                connection.Open();
                command.CommandText = $"SELECT * FROM post";
                using ( var reader = command.ExecuteReader())
                {
                    if(reader.HasRows)
                    {
                        while(reader.Read())
                        {
                            PostList.Add(

                                new PostModel
                                {
                                    Id = reader.GetInt32(0),
                                    Title = reader.GetString(1),
                                    Body =  reader.GetString(2),
                                    CreatedAt = reader.GetDateTime(3),
                                    UpdatedAt = reader.GetDateTime(4)

                                }
                            );
                        }
                    }
                    else
                    {
                        return new PostViewModel { PostList = PostList };
                    }
                }
            }
        }
        
        return new PostViewModel { PostList = PostList};
    
    }

    public ActionResult Insert(PostModel post)
    {
        post.CreatedAt = DateTime.Now;
        post.UpdatedAt = DateTime.Now;

         using (SqliteConnection connection = new SqliteConnection(_configuration.GetConnectionString("BlogDataContext")))
        {
            using ( var command = connection.CreateCommand() )
            {
                connection.Open();
                command.CommandText = $"INSERT INTO post(title, body, createdat, updatedat) VALUES('{post.Title}', '{post.Body}', '{post.CreatedAt}', '{post.UpdatedAt}')";
               try
               {
                command.ExecuteNonQuery();
               }catch(Exception ex)
               {
                Console.WriteLine(ex.Message);
               }
            }
        }
        return RedirectToAction("Index");
    }

       internal PostModel GetPostById(int id)
    {
        PostModel post = new();

        using (SqliteConnection connection = new SqliteConnection(_configuration.GetConnectionString("BlogDataContext")))
        {
            using ( var command = connection.CreateCommand() )
            {
                connection.Open();
                command.CommandText = $"SELECT * FROM post WHERE id = '{id}'";
                using ( var reader = command.ExecuteReader())
                {
                    if(reader.HasRows)
                    {
                        while(reader.Read())
                        {
                            
                                    post.Id = reader.GetInt32(0);
                                    post.Title = reader.GetString(1);
                                    post.Body =  reader.GetString(2);
                                    post.CreatedAt = reader.GetDateTime(3);
                                    post.UpdatedAt = reader.GetDateTime(4);

                                
                        }
                    }
                    else
                    {
                        return post;
                    }
                }
            }
        }
        
        return post;
    
    }

    public ActionResult Update(PostModel post)
    {
        post.UpdatedAt = DateTime.Now;
         using (SqliteConnection connection = new SqliteConnection(_configuration.GetConnectionString("BlogDataContext")))
        {
            using ( var command = connection.CreateCommand() )
            {
                connection.Open();
                command.CommandText = $"UPDATE post SET title = '{post.Title}', body = '{post.Body}', updatedat = '{post.UpdatedAt}' WHERE Id = '{post.Id}'";
               try
               {
                command.ExecuteNonQuery();
               }catch(Exception ex)
               {
                Console.WriteLine(ex.Message);
               }
            }
            return RedirectToAction("Index");
        }

    }

    [HttpPost]
    public JsonResult Delete(int Id)
    {
         using (SqliteConnection connection = new SqliteConnection(_configuration.GetConnectionString("BlogDataContext")))
        {
            using ( var command = connection.CreateCommand() )
            {
                connection.Open();
                command.CommandText = $"DELETE FROM post WHERE Id = '{Id}'";
               try
               {
                command.ExecuteNonQuery();
               }catch(Exception ex)
               {
                Console.WriteLine(ex.Message);
               }

            }
    }
    return Json(new Object{});
}
}
