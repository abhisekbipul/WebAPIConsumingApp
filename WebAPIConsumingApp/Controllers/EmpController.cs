using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using WebAPIConsumingApp.Models;

namespace WebAPIConsumingApp.Controllers
{
    public class EmpController : Controller
    {
        HttpClient client;
        public EmpController()
        {
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

            client = new HttpClient(clientHandler);
        }
        public IActionResult Index()
        {
           List<Emp> empList = new List<Emp>();
            string url = "https://localhost:7260/api/Emp/GetEmp";
            HttpResponseMessage response=client.GetAsync(url).Result;
            if(response.IsSuccessStatusCode)
            {
                var jsondata=response.Content.ReadAsStringAsync().Result;
                var obj=JsonConvert.DeserializeObject<List<Emp>>(jsondata);
                if(obj != null )
                {
                    empList = obj;
                }
            }
            return View(empList);
        }

        public IActionResult AddEmp()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddEmp(Emp emp)
        {
            string url = "https://localhost:7260/api/Emp/AddEmp";
            var jsondata=JsonConvert.SerializeObject(emp);
            StringContent stringContent = new StringContent(jsondata,Encoding.UTF8,"application/json");
            HttpResponseMessage result=client.PostAsync(url,stringContent).Result;
            if(result.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return View();
        }

        public IActionResult DelEmp(int id)
        {
            string url = "https://localhost:7260/api/Emp/DeleteEmp/";
            HttpResponseMessage result = client.DeleteAsync(url+id).Result;
            if (result.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }
        }

        public IActionResult UpdateEmp(int id)
        {
            string url = "https://localhost:7260/api/Emp/UpdateEmployee/{id}";
            HttpResponseMessage response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                var jsondata = response.Content.ReadAsStringAsync().Result;
                var emp = JsonConvert.DeserializeObject<Emp>(jsondata);
                if (emp != null)
                {
                    return View(emp);
                }
            }
            return NotFound();
        }

        [HttpPost]
        public IActionResult UpdateEmp(Emp emp)
        {
            string url = "https://localhost:7260/api/Emp/UpdateEmployee";
            var jsondata = JsonConvert.SerializeObject(emp);
            StringContent stringContent = new StringContent(jsondata, Encoding.UTF8, "application/json");
            HttpResponseMessage result = client.PutAsync(url, stringContent).Result;
            if (result.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return View(emp);
        }
    }
}
