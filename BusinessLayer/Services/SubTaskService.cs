using DataAccessLayer;
using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
    public class SubTaskService
    {
        private readonly ApplicationDbContext _context;

        public SubTaskService(ApplicationDbContext context)
        {
            _context = context;
        }

        public SubTask CreateSubTask(SubTask subTask)
        {
            _context.SubTasks.Add(subTask);
            _context.SaveChanges();

            return subTask;
        }
    }
}
