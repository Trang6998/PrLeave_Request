<template>
    <v-navigation-drawer persistent :mini-variant="miniVariant"
                         width="230"
                         :clipped="clipped"
                         id="appDrawer"
                         v-model="$store.state.app.drawer" enable-resize-watcher fixed app>

        <v-toolbar color="teal lighten-2" dark
                   height="50">
            <a href="https://giatlahp.com">
                <img v-bind:src="'https://giatlahp.com/images/LOGO%20HP%20NEW-01.png'" style="margin-top: 5px" height="36" alt="GiatLaHP">
            </a>
            <v-toolbar-title class="ml-0 pl-3">
                <span class="hidden-sm-and-down" style="font-size:20px;">Quản lý giặt là</span>
            </v-toolbar-title>
        </v-toolbar>
        <v-list id="leftSideBar">
            <vue-perfect-scrollbar class="drawer-menu--scroll" :settings="scrollSettings">
                <v-list dense expand>
                    <template v-if="dsMenu.length != 0" v-for="(item, i) in dsMenu">
                        <!--group with subitems-->
                        <v-list-group v-if="item.items && item.show" :key="item.name" :group="item.group" :prepend-icon="item.icon" no-action="no-action">
                            <v-list-tile slot="activator" ripple="ripple" v-if="item.link">
                                <v-list-tile-content>
                                    <v-list-tile-title>{{ item.title }}</v-list-tile-title>
                                </v-list-tile-content>
                            </v-list-tile>
                            <template v-for="(subItem, i) in item.items">
                                <!--sub group-->
                                <v-list-group v-if="subItem.items  && subItem.show" :key="subItem.name" :group="subItem.group" sub-group="sub-group">
                                    <v-list-tile slot="activator" ripple="ripple">
                                        <v-list-tile-content>
                                            <v-list-tile-title>{{ subItem.title }}</v-list-tile-title>
                                        </v-list-tile-content>
                                    </v-list-tile>
                                    <v-list-tile v-for="(grand, i) in subItem.children" :key="i" :to="grand.link" :href="grand.href" ripple="ripple">
                                        <v-list-tile-content>
                                            <v-list-tile-title>{{ grand.title }}</v-list-tile-title>
                                        </v-list-tile-content>
                                    </v-list-tile>
                                </v-list-group>
                                <!--child item-->
                                <v-list-tile v-else-if="subItem.show" :key="i" :to="subItem.link" :href="subItem.href" :disabled="subItem.disabled" :target="subItem.target" ripple="ripple">
                                    <v-list-tile-content>
                                        <v-list-tile-title><span>{{ subItem.title }}</span></v-list-tile-title>
                                    </v-list-tile-content>
                                    <!-- <v-circle class="white--text pa-0 circle-pill" v-if="subItem.badge" color="red" disabled="disabled">{{ subItem.badge }}</v-circle> -->
                                    <v-list-tile-action v-if="subItem.action">
                                        <v-icon :class="[subItem.actionClass || 'success--text']">{{ subItem.action }}</v-icon>
                                    </v-list-tile-action>
                                </v-list-tile>
                            </template>
                        </v-list-group>
                        <v-subheader v-else-if="item.header && item.show" :key="i">{{ item.header }}</v-subheader>
                        <v-divider v-else-if="item.divider && item.show" :key="i"></v-divider>
                        <!--top-level link-->
                        <v-list-tile v-else-if="item.show" :to="item.link" :href="item.href" ripple="ripple" :disabled="item.disabled" :target="item.target" rel="noopener" :key="item.name">
                            <v-list-tile-action v-if="item.icon">
                                <v-icon>{{ item.icon }}</v-icon>
                            </v-list-tile-action>
                            <v-list-tile-content v-if="item.link">
                                <v-list-tile-title>{{ item.title }}</v-list-tile-title>
                            </v-list-tile-content>
                            <v-list-tile-content v-if="item.href">
                                <a style="text-decoration:none;color:#20B2AA" :href="item.href" target="_blank"><v-list-tile-title>{{ item.title }}</v-list-tile-title></a>
                            </v-list-tile-content>
                            <!-- <v-circle class="white--text pa-0 chip--x-small" v-if="item.badge" :color="item.color || 'primary'" disabled="disabled">{{ item.badge }}</v-circle> -->
                            <v-list-tile-action v-if="item.subAction">
                                <v-icon class="success--text">{{ item.subAction }}</v-icon>
                            </v-list-tile-action>
                            <!-- <v-circle class="caption blue lighten-2 white--text mx-0" v-else-if="item.chip" label="label" small="small">{{ item.chip }}</v-circle> -->
                        </v-list-tile>
                    </template>
                </v-list>
            </vue-perfect-scrollbar>
        </v-list>
    </v-navigation-drawer>
</template>

<script>
    import VuePerfectScrollbar from 'vue-perfect-scrollbar';

    export default {
        name: 'App',
        components: { VuePerfectScrollbar },
        data() {
            return {
                scrollSettings: {
                    maxScrollbarLength: 160
                },
                clipped: false,
                drawer: true,
                fixed: true,
                miniVariant: false,
                right: true,
                rightDrawer: false,
                title: '',
                user: this.$store.state.user.Profile,
                dsMenu: []
            };
        },
        computed: {
            isLoggedIn() {
                this.user = this.$store.state.user.Profile
                return this.$store.state.user
                    && this.$store.state.user.AccessToken
                    && this.$store.state.user.AccessToken.IsAuthenticated;
            }
        },
        mounted() {
            this.dsMenu = this.getDSMenu();
        },
        methods: {
            getDSMenu() {
                return [
                    //{
                    //    icon: 'chat',
                    //    title: 'Quản lý bài viết',
                    //    show: this.checkRole(4) || this.checkRole(7),
                    //    link: '#',
                    //    items: [
                    //        {
                    //            icon: 'fas fa-clipboard-list',
                    //            show: this.checkRole(7),
                    //            title: 'Chuyên mục bài viết',
                    //            link: '/chuyenmucbaiviet'
                    //        },
                    //        {
                    //            icon: 'fas fa-clipboard-list',
                    //            show: this.checkRole(4),
                    //            title: 'Bài viết',
                    //            link: '/baiviet'
                    //        }
                    //    ]
                    //},
                    {
                        icon: 'flag',
                        title: 'Quản lý hình thức giặt',
                        show: this.user.LoaiTaiKhoanID == 4 || this.user.LoaiTaiKhoanID ==2,
                        link: '/hinhthucgiat'
                    },
                    {
                        icon: 'monetization_on',
                        title: 'Quản lý đơn giá',
                        show: this.user.LoaiTaiKhoanID == 4 || this.user.LoaiTaiKhoanID ==2,
                        link: '/dongia'
                    },
                    {
                        icon: 'mode_edit',
                        title: 'Quản lý loại đồ giặt',
                        show: this.user.LoaiTaiKhoanID == 4 || this.user.LoaiTaiKhoanID ==2,
                        link: '/loaidogiat'
                    },
                    {
                        icon: 'view_list',
                        title: 'Quản lý đồ giặt',
                        show: this.user.LoaiTaiKhoanID == 4 || this.user.LoaiTaiKhoanID ==2,
                        link: '/dogiat'
                    },
                    {
                        icon: 'supervisor_account',
                        title: 'Quản lý người dùng',
                        show: this.user.LoaiTaiKhoanID == 4 || this.user.LoaiTaiKhoanID ==2,
                        link: '/nguoidung'
                    },
                    {
                        icon: 'mood',
                        title: 'Quản lý đơn đặt hàng',
                        show: true,
                        link: '/khachdathang'
                    },
                    {
                        icon: 'assignment_turned_in',
                        title: 'Quản lý feedback người dùng',
                        show: this.user.LoaiTaiKhoanID == 4 || this.user.LoaiTaiKhoanID ==2,
                        link: '/lienhe'
                    },
                    {
                        icon: 'insert_chart',
                        title: 'Thống kê lượt truy cập',
                        show: this.user.LoaiTaiKhoanID == 4,
                        link: '/luottruycap'
                    },
                    {
                        icon: 'account_circle', 
                        title: 'Quản lý tài khoản',
                        show: this.user.LoaiTaiKhoanID == 4,
                        link: '/users'
                    },
                ];
            },
        }
    };
</script>

<style>
    #leftSideBar .v-list__group__header .v-list__group__header__prepend-icon,
    #leftSideBar .v-list__tile__action {
        min-width: 26px;
    }

    #leftSideBar .v-list__group__header .v-list__group__header__prepend-icon,
    #leftSideBar .v-list__tile__action {
        padding: 0 5px;
    }

    #leftSideBar .v-list__group__items .v-list__tile__title {
        padding: 0 10px 0 0;
    }

    #leftSideBar .v-list__tile {
        padding: 0;
    }

    #leftSideBar .v-list__group__header .v-list__group__header__append-icon {
        padding: 0 5px;
    }

    #leftSideBar .v-list__group__header .v-list__group__header__append-icon,
    #leftSideBar .v-list__group__header .v-list__group__header__prepend-icon {
        padding: 0 5px !important;
    }
</style>