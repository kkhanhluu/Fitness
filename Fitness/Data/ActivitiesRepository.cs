using Fitness.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fitness.Data
{
	public class ActivitiesRepository
	{
		public List<Activity> GetActivities()
        {
            return Data.Activities.OrderBy(a => a.Name).ToList();
        }
	}
}