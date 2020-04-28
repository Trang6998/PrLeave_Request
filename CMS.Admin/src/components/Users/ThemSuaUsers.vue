<template>
    <v-dialog v-model="isShow" width="600" scrollable persistent>
        <v-card>
            <v-card-title class="teal lighten-2 white--text pa-2">
                <span class="title">{{ isUpdate? 'Cập nhập tài khoản' : 'Thêm mới tài khoản' }}</span>
                <v-spacer></v-spacer>
                <v-btn class="white--text ma-0" small icon @click="hide">
                    <v-icon>close</v-icon>
                </v-btn>
            </v-card-title>
            <v-card-text>
                <v-form scope="frmAddEdit">
                    <v-layout row wrap>
                        <v-flex xs12 md6>
                            <v-text-field v-model="users.UserName"
                                          label="Tài khoản *"
                                          type="text"
                                          :error-messages="errors.collect('Tài khoản', 'frmAddEdit')"
                                          v-validate="'required'" class="ml-1 mr-1"
                                          data-vv-scope="frmAddEdit"
                                          data-vv-name="Tài khoản"
                                          clearable></v-text-field>
                        </v-flex>

                        <v-flex xs12 md6>
                            <v-text-field v-model="users.Password"
                                          label="Mật khẩu"
                                          type="text"
                                           :error-messages="errors.collect('Mật khẩu', 'frmAddEdit')"
                                          v-validate="'required'" class="ml-1 mr-1"
                                          data-vv-scope="frmAddEdit"
                                          data-vv-name="Mật khẩu"
                                          clearable></v-text-field>
                        </v-flex>

                        <v-flex xs12 md6>
                            <v-text-field v-model="users.Email"
                                          label="Email"
                                          type="text"
                                          :error-messages="errors.collect('Email', 'frmAddEdit')"
                                          v-validate="''" class="ml-1 mr-1"
                                          data-vv-scope="frmAddEdit"
                                          data-vv-name="Email"
                                          hide-details
                                          clearable></v-text-field>
                        </v-flex>

                        <v-flex xs12 md6>
                            <v-autocomplete v-model="users.LoaiTaiKhoanID"
                                            placeholder="Chọn loại tài khoản"
                                            label="Loại tài khoản"
                                            class="ml-1 mr-1"
                                            @change="getDataFromApi(searchParamsDoGiat)"
                                            :items="dsLoaiTaiKhoan"
                                            item-text="TenLoai"
                                            item-value="LoaiTaiKhoanID" 
                                            clearable></v-autocomplete>
                        </v-flex>
                        <v-flex xs12 md6>
                            <v-checkbox v-model="users.Active" label="Hoạt động"
                                        :error-messages="errors.collect('Hoạt động', 'frmAddEdit')"
                                        v-validate="''"
                                        data-vv-scope="frmAddEdit"
                                        data-vv-name="Hoạt động"></v-checkbox>
                        </v-flex>

                    </v-layout>
                </v-form>
            </v-card-text>
            <v-card-actions>
                <v-spacer></v-spacer>
                <v-btn text @click.native="hide">Hủy</v-btn>
                <v-btn text @click.native="save" color="teal lighten-2"
                       :loading="loadingSave">Lưu</v-btn>
            </v-card-actions>
        </v-card>
    </v-dialog>
</template>

<script lang="ts">
    import { Vue } from 'vue-property-decorator';
    import UsersApi, { UsersApiSearchParams } from '@/apiResources/UsersApi';
    import LoaiTaiKhoanApi, { LoaiTaiKhoanApiSearchParams } from '@/apiResources/LoaiTaiKhoanApi';
    import { Users } from '@/models/Users';
    import { LoaiTaiKhoan } from '@/models/LoaiTaiKhoan';
    export default Vue.extend({
        $_veeValidate: {
            validator: 'new'
        },
        components: {},
        data() {
            return {
                dsLoaiTaiKhoan: [] as LoaiTaiKhoan[],
                isShow: false,
                isUpdate: false,
                users: {} as Users,
                loading: false,
                loadingSave: false,
                searchParamsUsers: {} as UsersApiSearchParams,
                searchParamsLoaiTaiKhoan: { rowsPerPage: 0 } as LoaiTaiKhoanApiSearchParams,
                active: [] as any[],
                open: [],
                usersID: 0,
            }
        },
        watch: {
        },
        methods: {
            hide() {
                this.isShow = false
            },
            show(isUpdate: boolean, item: any) {
                this.$validator.reset();
                this.loadingSave = false;
                this.isShow = true;
                this.isUpdate = isUpdate;
                this.getLoaiTaiKhoan();
                if (isUpdate) {
                    this.usersID = item.UserId;
                    this.getDataFromApi(this.usersID);
                }
                else {
                    this.users = {} as Users;
                }
            },
            getDataFromApi(id: number): void {
                UsersApi.detail(id).then(res => {
                    this.users = res;
                    this.users.Password = '';
                });
            },
            getLoaiTaiKhoan(): void {
                LoaiTaiKhoanApi.search(this.searchParamsLoaiTaiKhoan).then(res => {
                    this.dsLoaiTaiKhoan = res.Data;
                })
            },
            save(): void {
                this.$validator.validateAll('frmAddEdit').then((res) => {
                    if (res) {
                        if (this.isUpdate) {
                            this.loading = true;
                            UsersApi.update(this.usersID, this.users).then(res => {
                                this.loading = false;
                                this.$emit("save");
                                this.isShow = false;
                                this.$snotify.success('Cập nhật thành công!');
                            }).catch(res => {
                                this.loading = false;
                                this.$snotify.error('Cập nhật thất bại!');
                            });
                        } else {
                            this.loading = true;
                            UsersApi.insert(this.users).then(res => {
                                this.users = res;
                                this.users.Password = '';
                                this.isShow = false;
                                this.$emit("save");
                                this.loading = false;
                                this.$snotify.success('Thêm mới thành công!');
                            }).catch(res => {
                                this.loading = false;
                                this.$snotify.error('Thêm mới thất bại!');
                            });
                        }
                    }
                });
            },
        }
    });
</script>

