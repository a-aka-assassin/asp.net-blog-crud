// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function deletePost(i)
{
    $.ajax({
        url: '/Post/Delete',
        type: 'Post',
        data: {
            id: i
        }
    }).then(function(data)
    {
        window.location = "/Post";
    })
}