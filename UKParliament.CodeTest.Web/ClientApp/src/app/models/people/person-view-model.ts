import {DepartmentViewModel} from "@/app/models/departments/department-view-model";

export interface PersonViewModel {
  id: number;
  firstName: string;
  lastName: string;
  email: string;
  dateOfBirth: Date;
  department: DepartmentViewModel;
}
