using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

// using statements required for EF DB access
using COMP2007_S2016_MidTerm_200277628.Models;
using System.Web.ModelBinding;

/**
  * @author: Yandong Wang 200277628
  * @date: June 23, 2016
  * @version： 0.0.3
  * @file description: create a web app that show the todo list and list details
  */


namespace COMP2007_S2016_MidTerm_200277628
{
    public partial class TodoDetails : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if ((!IsPostBack) && (Request.QueryString.Count > 0))
            {
                this.GetTodo();
            }
        }

        /**
         * <summary>
         * This method gets the todo data from the DB
         * </summary>
         * 
         * @method GetTodos
         * @returns {void}
         */
        protected void GetTodo()
        {
            // populate teh form with existing data from the database
            int TodoID = Convert.ToInt32(Request.QueryString["TodoID"]);

            // connect to the EF DB
            using (TodoConnection db = new TodoConnection())
            {
                // populate a Todo object instance with the TodoID from the URL Parameter
                Todo updatedTodo = (from todo in db.Todos
                                       where todo.TodoID == TodoID
                                       select todo).FirstOrDefault();

                // map the todo properties to the form controls
                if (updatedTodo != null)
                {
                    TodoNameTextBox.Text = updatedTodo.TodoName;
                    TodoNotesTextBox.Text = updatedTodo.TodoNotes;
                   
                }
            }
        }

        protected void CancelButton_Click(object sender, EventArgs e)
        {
            // Redirect back to Todos page
            Response.Redirect("~/TodosList.aspx");
        }
        /**
         * <summary>
         * This event handler allows save the data for the TodosList page
         * </summary>
         * 
         * @method SaveButton_Click1
         * @param {object} sender
         * @param {GridViewPageEventArgs} e
         * @returns {void}
         */
        protected void SaveButton_Click1(object sender, EventArgs e)
        {
            // Use EF to connect to the server
            using (TodoConnection db = new TodoConnection())
            {
                // use the Todo model to create a new todo object and
                // save a new record
                Todo newTodo = new Todo();

                int TodoID = 0;

                if (Request.QueryString.Count > 0) // our URL has a TodoID in it
                {
                    // get the id from the URL
                    TodoID = Convert.ToInt32(Request.QueryString["TodoID"]);

                    // get the current todo from EF DB
                    newTodo = (from todo in db.Todos
                               where todo.TodoID == TodoID
                               select todo).FirstOrDefault();
                }

                // add form data to the new todo record
                newTodo.TodoName = TodoNameTextBox.Text;

                newTodo.TodoNotes = TodoNotesTextBox.Text;


                // use LINQ to ADO.NET to add / insert new todo into the database

                if (TodoID == 0)
                {
                    db.Todos.Add(newTodo);
                }


                // save our changes - also updates and inserts
                db.SaveChanges();

                // Redirect back to the updated students page
                Response.Redirect("~/TodosList.aspx");
            }
        }
    }
}