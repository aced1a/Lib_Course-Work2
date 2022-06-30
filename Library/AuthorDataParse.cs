using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net;
using System.Text.Json;
using Library.Model.LibraryEntities;

namespace Library
{
    class AuthorDataParse
    {
        public Book Book { get; private set; }
        public List<Author> Authors { get; private set; }
        public Publisher Publisher { get; private set; }

        public AuthorDataParse()
        {
            Book = new Book();
            Authors = new List<Author>();
            Publisher = new Publisher();
        }

        public bool GetAuthorByISBN(string isbn)  
        {
            try
            {

                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                    using (var response = client.GetAsync($"https://www.googleapis.com/books/v1/volumes?q=isbn:{isbn}").Result)
                    {
                        response.EnsureSuccessStatusCode();
                        var result = response.Content.ReadAsStringAsync();
                        JsonDocument json = JsonDocument.Parse(result.Result);

                        if (json.RootElement.GetProperty("totalItems").GetInt32() != 0)
                        {
                            GetItems(json);  
                        }
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }


        void GetItems(JsonDocument json)
        {
            foreach (var item in json.RootElement.GetProperty("items").EnumerateArray())
            {
                var info = item.GetProperty("volumeInfo");
                GetTitle(info); GetDescription(info); GetAuthors(info); GetPublisher(info); GetImage(info);
                break;
            }
        }

        void GetImage(JsonElement element)
        {
            JsonElement image, thumbnail;
            if(element.TryGetProperty("imageLinks", out image))
            {
                if(image.TryGetProperty("thumbnail", out thumbnail)){
                    LoadImage(thumbnail.GetString());
                }
            }
        }

        void LoadImage(string url) 
        {
            try
            {
                var request = WebRequest.Create(url);
                var response = request.GetResponse();
                using (var stream = response.GetResponseStream())
                {
                    Book.Image = new byte[9999999];
                    var r = stream.Read(Book.Image, 0, 9999999);
                }
            }
            catch (Exception) { }
        }

        void GetPublisher(JsonElement element) 
        {
            JsonElement publisher;
            if(element.TryGetProperty("publisher", out publisher))
            {
                Publisher.Name = publisher.GetString();
            }
        }

        void GetTitle(JsonElement element)
        {
            JsonElement subtitle;
            Book.Title = element.GetProperty("title").GetString();
            if (element.TryGetProperty("subtitle", out subtitle))
            {
               Book.Title += " " + subtitle.GetString();
            }
        }

        void GetDescription(JsonElement element)
        {
            JsonElement desc;
            if (element.TryGetProperty("description", out desc)) 
            {
                Book.Description = desc.GetString();
            }
        }

        void GetAuthors(JsonElement element)
        {
            JsonElement authors;
            if (element.TryGetProperty("authors", out authors))
            {
                foreach (var item in authors.EnumerateArray())
                {

                    string name = item.GetString();
                    string[] names = name.Split();
                    if (names.Length > 0)
                    {
                        string first = names[0];
                        string last = names.Length > 1 ? (name.Substring(name.IndexOf(names[0]) + names[0].Length)) : null;
                        Authors.Add(
                            new Author()
                            {
                                FirstName = first,
                                LastName = last
                            }
                        );
                    }
                }
            }
        }


    }
}
