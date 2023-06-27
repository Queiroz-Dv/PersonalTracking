﻿using PersonalTracking.DAL.DataAcess;
using System.Collections.Generic;

namespace PersonalTracking.DAL.DTO
{
    public class EmployeeDTO
    {
        public List<DEPARTMENT> Departments { get; set; }

        public List<PositionDTO> Positions { get; set; }

        public List<EmployeeDetailDTO> Employees { get; set; }
    }
}
