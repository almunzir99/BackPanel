import { MenuGroup } from "./menu.group";

export const MenuList: MenuGroup[] = [
    {
        title: "General",
        children: [
            {
                title: "Home",
                icon: "las la-home",
                route: "/dashboard/home",
                permissionName:"generalPermissions"

            },
            {
                title: "Admins",
                icon: "las la-user-tie",
                route: "/dashboard/admins",
                permissionName:"adminsPermissions"
            },
            {
                title: "Roles",
                icon: "las la-users-cog",
                route: "/dashboard/roles",
                permissionName:"rolesPermissions"


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
                permissionName:"messagesPermissions"


            },
        ]
    },
    {
        title: "More",
        children: [

            {
                title: "Files Manager",
                icon: "las la-folder-open",
                route: "/dashboard/files-manager",
                permissionName:"generalPermissions"


            },
            {
                title: "Translation Editor",
                icon: "las la-globe-europe",
                route: "/dashboard/translation-editor",
                permissionName:"generalPermissions"


            },


            {
                title: "Profile",
                icon: "las la-user-cog",
                route: "/dashboard/profile",
                permissionName:"generalPermissions"

                 

            },
            {
                title: "Settings",
                icon: "las la-cog",
                route: "/dashboard/settings",
                permissionName:"generalPermissions"


            },
        ]
    }
];