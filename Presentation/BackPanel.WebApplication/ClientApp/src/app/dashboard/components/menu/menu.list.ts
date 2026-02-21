import { MenuGroup } from "./menu.group";

export const MenuList: MenuGroup[] = [
    {
        title: "General",
        children: [
            {
                title: "Home",
                icon: "las la-home",
                route: "/dashboard/home",
                allowedRoles: []

            },
            {
                title: "Admins",
                icon: "las la-user-tie",
                route: "/dashboard/admins",
                allowedRoles: ["Admins", "Administrator", "SuperAdmin"]
            },
            {
                title: "Roles",
                icon: "las la-users-cog",
                route: "/dashboard/roles",
                allowedRoles: ["Roles", "Administrator", "SuperAdmin"]


            }
        ]
    },
    {
        title: "Pages",
        children: [

            {
                title: "Messages",
                icon: "las la-envelope",
                route: "/dashboard/messages",
                allowedRoles: ["Messages", "Administrator", "SuperAdmin"]


            },
        ]
    },
    {
        title: "More",
        children: [

            {
                title: "Files-Manager",
                icon: "las la-folder-open",
                route: "/dashboard/files-manager",
                allowedRoles: []


            },
            {
                title: "Translation-Editor",
                icon: "las la-globe-europe",
                route: "/dashboard/translation-editor",
                allowedRoles: []


            },


            {
                title: "Profile",
                icon: "las la-user-cog",
                route: "/dashboard/profile",
                allowedRoles: []



            },
            {
                title: "Company-Information",
                icon: "las la-business-time",
                route: "/dashboard/company-info",
                allowedRoles: ["CompanyInfo", "Administrator", "SuperAdmin"]
            }
        ]
    }
];