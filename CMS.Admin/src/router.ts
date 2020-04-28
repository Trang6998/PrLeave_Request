import Vue from 'vue';
import Router, { Route } from 'vue-router';
import store from './store/store';
import { HTTP } from '@/HTTPServices';
import Login from './components/Login/Login.vue';

import DanhSachLuotTruyCap from './components/LuotTruyCap/DanhSachLuotTruyCap.vue';
import ThongKeLuotTruyCap from './components/LuotTruyCap/ThongKeLuotTruyCap.vue';
import DanhSachUsers from './components/Users/DanhSachUsers.vue';
import ThemSuaUsers from './components/Users/ThemSuaUsers.vue';
import DanhSachHinhThucGiat from './components/HinhThucGiat/DanhSachHinhThucGiat.vue';
import DanhSachDonGia from './components/DonGia/DanhSachDonGia.vue';
import DanhSachChiTietDoGiat from './components/ChiTietDoGiat/DanhSachChiTietDoGiat.vue';
import DanhSachDoGiat from './components/DoGiat/DanhSachDoGiat.vue';
import DanhSachLoaiDoGiat from './components/LoaiDoGiat/DanhSachLoaiDoGiat.vue';
import DanhSachNguoiDung from './components/NguoiDung/DanhSachNguoiDung.vue';
import DanhSachKhachDatHang from './components/KhachDatHang/DanhSachKhachDatHang.vue';
import DanhSachLienHe from './components/LienHe/DanhSachLienHe.vue';
import DanhSachDonXinNghi from './components/DonXinNghi/DanhSachDonXinNghi.vue';
import Home from './components/Home.vue';

Vue.use(Router);
export default new Router({
    routes: [

        {
            path: '/login',
            name: 'login',
            component: Login,
        },
        {
            path: '/',
            name: 'home',
            component: Home,
        },

        {
            path: '/luottruycap',
            name: 'luotTruyCap',
            component: DanhSachLuotTruyCap,
            beforeEnter: guardRoute,
        },
        {
            path: '/thongkeluottruycap',
            name: 'thongKeLuotTruyCap',
            component: ThongKeLuotTruyCap,
            beforeEnter: guardRoute,
        },
        {
            path: '/users',
            name: 'users',
            component: DanhSachUsers,
            beforeEnter: guardRoute
        },
        {
            path: '/users/add',
            name: 'themUsers',
            component: ThemSuaUsers,
            beforeEnter: guardRoute
        },
       
        {
            path: '/hinhthucgiat',
            name: 'hinhThucGiat',
            component: DanhSachHinhThucGiat,
            beforeEnter: guardRoute
        },
        {
            path: '/dongia',
            name: 'donGia',
            component: DanhSachDonGia,
            beforeEnter: guardRoute
        },
        {
            path: '/chitietdogiat',
            name: 'chiTietDoGiat',
            component: DanhSachChiTietDoGiat,
            beforeEnter: guardRoute
        },
        {
            path: '/dogiat',
            name: 'doGiat',
            component: DanhSachDoGiat,
            beforeEnter: guardRoute
        },
        {
            path: '/loaidogiat',
            name: 'loaiDoGiat',
            component: DanhSachLoaiDoGiat,
            beforeEnter: guardRoute
        },
        {
            path: '/khachdathang',
            name: 'khachDatHang',
            component: DanhSachKhachDatHang,
            beforeEnter: guardRoute
        },
        {
            path: '/nguoidung',
            name: 'nguoiDung',
            component: DanhSachNguoiDung,
            beforeEnter: guardRoute
        },
        {
            path: '/lienhe',
            name: 'lienHe',
            component: DanhSachLienHe,
            beforeEnter: guardRoute
        },
        {
            path: '/donxinnghi',
            name: 'donxinnghi',
            component: DanhSachDonXinNghi,
            beforeEnter: guardRoute
        },
    ],
});


function guardRoute(to: Route, from: Route, next: any): void {
    // work-around to get to the Vuex store (as of Vue 2.0)
    const isAuthenticated = store.state.user && store.state.user.AccessToken ? store.state.user.AccessToken.IsAuthenticated : false;
    if (!isAuthenticated) {
        next({
            path: '/login',
            query: {
                redirect: to.fullPath
            }
        });
    } else {
        HTTP.get('/auth/validate-token/')
            .then(response => {
                next();
            })
            .catch(e => {
                store.commit('CLEAR_ALL_DATA');
                next({
                    path: '/login',
                    query: {
                        redirect: to.fullPath
                    }
                });
            });
    }
}