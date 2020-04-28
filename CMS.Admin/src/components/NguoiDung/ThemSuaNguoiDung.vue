<template>
    <v-dialog v-model="isShow" width="600" scrollable persistent>
        <v-card>
            <v-card-title class="teal lighten-2 white--text pa-2">
                <span class="title">{{ isUpdate? 'Cập nhập người dùng' : 'Thêm mới người dùng' }}</span>
                <v-spacer></v-spacer>
                <v-btn class="white--text ma-0" small icon @click="hide">
                    <v-icon>close</v-icon>
                </v-btn>
            </v-card-title>
            <v-card-text>
                <v-form ref="form" v-model="valid" scope="formNguoiDung">
                    <v-layout row wrap>
                        <v-flex xs12 md6>
                            <v-text-field v-model="nguoiDung.TenNguoiDung"
                                          label="Tên người dùng"
                                          :counter="50"
                                          :error-messages="errors.collect('Tên người dùng', 'formNguoiDung')"
                                          v-validate="'required|max:50'"
                                          data-vv-scope="formNguoiDung" class="ml-1 mr-1"
                                          data-vv-name="Tên người dùng">
                            </v-text-field>
                        </v-flex>
                        <v-flex xs12 md6>
                            <v-text-field v-model="nguoiDung.SoDienThoai"
                                          label="Số điện thoại" class="ml-1 mr-1"
                                          :counter="15"
                                          :error-messages="errors.collect('Số điện thoại', 'formNguoiDung')"
                                          v-validate="'required|max:15|numeric'"
                                          data-vv-scope="formNguoiDung"
                                          data-vv-name="Số điện thoại">
                            </v-text-field>
                        </v-flex>
                        <v-flex xs12 md6>
                            <v-text-field v-model="nguoiDung.SoKhac"
                                          label="Số điện thoại khác" class="ml-1 mr-1"
                                          :counter="15"
                                          :error-messages="errors.collect('Số điện thoại khác', 'formNguoiDung')"
                                          v-validate="'max:15|numeric'"
                                          data-vv-scope="formNguoiDung"
                                          data-vv-name="Số điện thoại khác">
                            </v-text-field>
                        </v-flex>
                        <v-flex xs12 md6>
                            <v-text-field v-model="nguoiDung.DiaChi"
                                          label="Địa chỉ" class="ml-1 mr-1"
                                          :counter="200"
                                          v-validate="'required|max:200'"
                                          :error-messages="errors.collect('Địa chỉ', 'formNguoiDung')"
                                          data-vv-scope="formNguoiDung"
                                          data-vv-name="Địa chỉ">
                            </v-text-field>
                        </v-flex>
                        <v-flex xs12 md6>
                            <v-text-field v-model="nguoiDung.TaiKhoan"
                                          label="Tài khoản" class="ml-1 mr-1"
                                          :counter="100"
                                          :error-messages="errors.collect('Tài khoản', 'formNguoiDung')"
                                          data-vv-scope="formNguoiDung"
                                          data-vv-name="Tài khoản">
                            </v-text-field>
                        </v-flex>
                        <v-flex xs12 md6>
                            <v-text-field v-model="nguoiDung.Facebook"
                                          label="Facebook" class="ml-1 mr-1"
                                          :counter="100"
                                          :error-messages="errors.collect('Facebook', 'formNguoiDung')"
                                          data-vv-scope="formNguoiDung"
                                          data-vv-name="Facebook">
                            </v-text-field>
                        </v-flex>
                        <v-flex xs12 md6>
                            <v-text-field v-model="nguoiDung.Gmail"
                                          label="Gmail" class="ml-1 mr-1"
                                          :counter="100"
                                          :error-messages="errors.collect('Gmail', 'formNguoiDung')"
                                          data-vv-scope="formNguoiDung"
                                          data-vv-name="Gmail">
                            </v-text-field>
                        </v-flex>
                        <v-flex xs12 md6>
                            <v-text-field v-model="nguoiDung.DiemThuong" type="number"
                                          label="Điểm thưởng" class="ml-1 mr-1" :readonly="true"
                                          :error-messages="errors.collect('Điểm thưởng', 'formNguoiDung')"
                                          v-validate="'numeric|min:0'"
                                          data-vv-scope="formNguoiDung"
                                          data-vv-name="Điểm thưởng">
                            </v-text-field>
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
    import NguoiDungApi, { NguoiDungApiSearchParams } from '@/apiResources/NguoiDungApi';
    import { NguoiDung } from '@/models/NguoiDung';

    export default Vue.extend({
        $_veeValidate: {
            validator: 'new'
        },
        components: {},
        data() {
            return {
                isUpdate: false,
                nguoiDung: {} as NguoiDung,
                valid: false,
                isShow: false,
                loading: false,
                loadingSave: false,
                nguoiDungID: 0,
                searchParamsNguoiDung: {} as NguoiDungApiSearchParams,
            }
        },
        watch: {
        },
        methods: {
            hide() {
                this.isShow = false
            },
            show(isUpdate: boolean, item: any) {
                this.isShow = true;
                this.$validator.reset();
                this.loadingSave = false;
                this.isUpdate = isUpdate;
                if (isUpdate) {
                    this.nguoiDungID = item.NguoiDungID;
                    this.getDataFromApi(this.nguoiDungID);
                }
                else {
                    this.nguoiDung = {} as NguoiDung;
                }
            },
            getDataFromApi(id: number): void {
                NguoiDungApi.detail(id).then(res => {
                    this.nguoiDung = res;
                });
            },
            save(): void {
                this.$validator.validateAll('formNguoiDung').then((res) => {
                   if (res) {
                        if (this.isUpdate) {
                            this.loading = true;
                            this.loadingSave = true;
                            NguoiDungApi.update(this.nguoiDungID, this.nguoiDung).then(res => {
                                this.loading = false;
                                this.isShow = false;
                                this.loadingSave = false;
                                this.$emit("save");
                                this.$snotify.success('Cập nhật thành công!');
                            }).catch(res => {
                                this.loading = false;
                                this.loadingSave = true;
                                this.$snotify.error('Cập nhật thất bại!');
                            });
                        } else {
                            this.loading = true;
                            this.loadingSave = true;
                            NguoiDungApi.insert(this.nguoiDung).then(res => {
                                this.nguoiDung = res;
                                this.isUpdate = true;
                               this.loadingSave = false;
                                this.loading = false;
                                this.isShow = false;
                                this.$emit("save");
                                this.$snotify.success('Thêm mới thành công!');
                            }).catch(res => {
                                this.loading = false;
                                   this.loadingSave = true;
                                this.$snotify.error('Thêm mới thất bại!');
                            });
                        }
                    }
                });
            }
        }
    });
</script>

