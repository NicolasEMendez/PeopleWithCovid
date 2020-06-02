using Common;
using Services;
using System;
using System.Collections.Generic;

namespace CovidConsole
{
    class MenuCreator
    {
        private CollectionMenu menuCollection;
        private readonly ScreenService screenService;
        private readonly LocalizationService locService;
        private readonly PersonService personService;

        public MenuCreator()
        {
            menuCollection = new CollectionMenu();
            locService = new LocalizationService();
            screenService = new ScreenService(locService);
            personService = new PersonService(locService);
        }
        
        /// <summary>
        /// Creates a collection of menus that has a list of menus
        /// </summary>
        /// <returns></returns>
        public CollectionMenu CreateCollectionMenu()
        {
            screenService.ClearScreen();

            menuCollection = new CollectionMenu()
            {
                InvalidSelection = locService.GetString("Menu_Invalid_Selection"),
                SelectOption = locService.GetString("Menu_Select_An_Option"),
                Menus =
                {
                    CreateChooseLanguageMenu(),
                    CreatePrincipalMenu(),
                    ShowPatients(),
                    ShowCovidStatus(),
                    AddCovidPatientMenu(),
                    ModifyCovidPatientMenu(),
                    DeleteCovidPatientMenu()
                }
            };
            return menuCollection;
        }

        /// <summary>
        /// Creates the select language Menu
        /// </summary>
        /// <returns></returns>
        private Menu CreateChooseLanguageMenu()
        {
            Menu menu = new Menu()
            {
                MenuId = 0,
                Description = locService.GetString("Please_Select_Language").ToUpper(),
                MenuItems =
                {
                    new MenuItem()
                    {
                        Description = locService.GetString("Language_Selected_English"),
                        Actions = new List<Action>() { () => locService.ChangeLanguage(Languages.en), () => CreateCollectionMenu(), () => menuCollection.ShowMenu(1) },
                    },
                    new MenuItem()
                    {
                        Description = locService.GetString("Language_Selected_Spanish"),
                        Actions = new List<Action>() { () => locService.ChangeLanguage(Languages.es), () => CreateCollectionMenu(), () => menuCollection.ShowMenu(1) },
                    }
                }
            };

            return menu;
        }

        /// <summary>
        /// Creates the Main Menu
        /// </summary>
        /// <returns></returns>
        private Menu CreatePrincipalMenu()
        {
            return new Menu()
            {
                MenuId = 1,
                Description = $"{ locService.GetString("Welcome_To_Hospital").ToUpper() }, { locService.GetString("Please_Select_Option").ToUpper() }",
                MenuItems =
                {
                    new MenuItem()
                    {
                        Description = locService.GetString("Show_Patients"),
                        SubMenuId = 2
                    },
                    new MenuItem()
                    {
                        Description = locService.GetString("Show_Add_Patients"),
                        SubMenuId = 4
                    },
                    new MenuItem()
                    {
                        Description = locService.GetString("Show_Modify_Patients"),
                        SubMenuId = 5
                    },
                    new MenuItem()
                    {
                        Description = locService.GetString("Show_Delete_Patients"),
                        SubMenuId = 6
                    },
                    new MenuItem()
                    {
                        Description = locService.GetString("Menu_Language_Selector"),
                        SubMenuId = 0
                    },
                }
            };
        }

        /// <summary>
        /// Creates the "Show Patients" Menu
        /// </summary>
        /// <returns></returns>
        private Menu ShowPatients()
        {
            return new Menu()
            {
                MenuId = 2,
                Description = $"{ locService.GetString("Show_Patients_Word").ToUpper() }, { locService.GetString("Please_Select_Option").ToUpper() }",
                MenuItems =
                {
                    new MenuItem()
                    {
                        Description = locService.GetString("Show_All_Patients"),
                        SubMenuId = 2,
                        Actions = new List<Action>() { () => personService.ShowAllPatients() }
                    },
                    new MenuItem()
                    {
                        Description = locService.GetString("Show_Patients_Covid_Status"),
                        SubMenuId = 3,
                    },
                    new MenuItem()
                    {
                        Description = locService.GetString("Return_Main_Menu"),
                        SubMenuId = 1,
                    }
                }
            };
        }

        /// <summary>
        /// Menu that Shows the covid statuses in order for the user to pick one and then show the people with that state
        /// </summary>
        /// <returns></returns>
        private Menu ShowCovidStatus()
        {
            return new Menu()
            {
                MenuId = 3,
                Description = $"{ locService.GetString("Show_Patients_With_Status_Covid").ToUpper() }, { locService.GetString("Please_Select_Option").ToUpper() }",
                MenuItems =
                {
                    new MenuItem()
                    {
                        SubMenuId = 2,
                        Description = locService.GetString("Covid_Status_Recuperated"),
                        Actions = new List<Action>() { () => personService.ShowPeopleWithCovidState(CovidState.Recuperated) }
                    },
                    new MenuItem()
                    {
                        SubMenuId = 2,
                        Description = locService.GetString("Covid_Status_UnderEvaluation"),
                        Actions = new List<Action>() { () => personService.ShowPeopleWithCovidState(CovidState.UnderEvaluation) }
                    },
                    new MenuItem()
                    {
                        SubMenuId = 2,
                        Description = locService.GetString("Covid_Status_AdmittedInHospital"),
                        Actions = new List<Action>() { () => personService.ShowPeopleWithCovidState(CovidState.AdmittedInHospital) }
                    },
                    new MenuItem()
                    {
                        SubMenuId = 2,
                        Description = locService.GetString("Covid_Status_Critical"),
                        Actions = new List<Action>() { () => personService.ShowPeopleWithCovidState(CovidState.Critical) }
                    },
                    new MenuItem()
                    {
                        SubMenuId = 2,
                        Description = locService.GetString("Covid_Status_Deceased"),
                        Actions = new List<Action>() { () => personService.ShowPeopleWithCovidState(CovidState.Deceased) }
                    },
                    new MenuItem()
                    {
                        Description = locService.GetString("Menu_GoBack"),
                        SubMenuId = 2,
                    },
                    new MenuItem()
                    {
                        Description = locService.GetString("Menu_Return_MainMenu"),
                        SubMenuId = 1,
                    }
                }
            };
        }

        /// <summary>
        /// Adds the menu to Add Patients
        /// </summary>
        /// <returns></returns>
        private Menu AddCovidPatientMenu()
        {
            return new Menu()
            {
                MenuId = 4,
                Description = $"{ locService.GetString("Show_Add_Patients").ToUpper() }, { locService.GetString("Please_Select_Option").ToUpper() }",
                MenuItems =
                {
                    new MenuItem()
                    {
                        Description = locService.GetString("Show_Add_Patients"),
                        SubMenuId = 1,
                        Actions = new List<Action>() { () => personService.AddPatient() }
                    },
                    new MenuItem()
                    {
                        Description = locService.GetString("Return_Main_Menu"),
                        SubMenuId = 1,
                    }
                }
            };
        }

        /// <summary>
        /// Adds the Menu to Modify patients
        /// </summary>
        /// <returns></returns>
        private Menu ModifyCovidPatientMenu()
        {
            return new Menu()
            {
                MenuId = 5,
                Description = $"{ locService.GetString("Show_Modify_Patients").ToUpper() }, { locService.GetString("Please_Select_Option").ToUpper() }",
                MenuItems =
                {
                    new MenuItem()
                    {
                        Description = locService.GetString("Show_Modify_Patients"),
                        SubMenuId = 1,
                        Actions = new List<Action>() { () => personService.ModifyPatient() }
                    },
                    new MenuItem()
                    {
                        Description = locService.GetString("Return_Main_Menu"),
                        SubMenuId = 1,
                    }
                }
            };
        }

        /// <summary>
        /// Adds a Menu that Deletes Patients
        /// </summary>
        /// <returns></returns>
        private Menu DeleteCovidPatientMenu()
        {
            return new Menu()
            {
                MenuId = 6,
                Description = $"{ locService.GetString("Show_Delete_Patients").ToUpper() }, { locService.GetString("Please_Select_Option").ToUpper() }",
                MenuItems =
                {
                    new MenuItem()
                    {
                        Description = locService.GetString("Show_Delete_Patients"),
                        SubMenuId = 1,
                        Actions = new List<Action>() { () => personService.DeletePatient() }
                    },
                    new MenuItem()
                    {
                        Description = locService.GetString("Return_Main_Menu"),
                        SubMenuId = 1,
                    }
                }
            };
        }

    }
}
