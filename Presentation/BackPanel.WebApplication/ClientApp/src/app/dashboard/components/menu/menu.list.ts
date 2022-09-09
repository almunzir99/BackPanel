import { MenuGroup } from "./menu.group";

export const MenuList: MenuGroup[] = [
    {
        title: "General",
        children: [
            {
                title: "Home",
                icon: "las la-home",
                route: "/dashboard/home"

            },
            {
                title: "Admins",
                icon: "las la-user-tie",
                route: "/dashboard/admins"
            },
            {
                title: "Roles",
                icon: "las la-users-cog",
                route: "/dashboard/roles"

            }
        ]
    },
    {
        title: "Pages",
        children: [

            {
                title: "Messages",
                icon: "las la-envelope",
                route: "/dashboard/messages"

            },
        ]
    },
    {
        title: "More",
        children: [

            {
                title: "Files Manager",
                icon: "las la-folder-open",
                route: "/dashboard/files-manager"

            },
            {
                title: "Translation Editor",
                icon: "las la-globe-europe",
                route: "/dashboard/translation-editor"

            },


            {
                title: "Profile",
                icon: "las la-user-cog",
                route: "/dashboard/profile"
                 

            },
            {
                title: "Settings",
                icon: "las la-cog",
                route: "/dashboard/settings"

            },
        ]
    }
];