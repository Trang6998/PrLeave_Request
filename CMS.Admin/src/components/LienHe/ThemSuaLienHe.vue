<template>
    <v-dialog v-model="isShow" width="600" scrollable persistent>
        <v-card>
            <v-card-title class="teal lighten-2 white--text pa-2">
                <span class="title">{{ isUpdate? 'Chi tiết feedback' : 'Thêm mới feedback' }}</span>
                <v-spacer></v-spacer>
                <v-btn class="white--text ma-0" small icon @click="hide">
                    <v-icon>close</v-icon>
                </v-btn>
            </v-card-title>
            <v-card-text>
                <v-form ref="form" v-model="valid" scope="formLienHe">
                    <v-layout row wrap>

                        <v-flex xs12 md6>
                            <v-text-field v-model="lienHe.HoTen"
                                          label="Họ tên"
                                          :counter="50"
                                          v-validate="'required|max:50'"
                                          :error-messages="errors.collect('Họ tên', 'formLienHe')"
                                          data-vv-scope="formLienHe" class="ml-1 mr-1"
                                          data-vv-name="Họ tên">
                            </v-text-field>
                        </v-flex>
                        <v-flex xs12 md6>
                            <v-text-field v-model="lienHe.SoDienThoai"
                                          label="Số điện thoại" class="ml-1 mr-1"
                                          :counter="20"
                                          v-validate="'required|max:20|numeric'"
                                          :error-messages="errors.collect('Số điện thoại', 'formLienHe')"
                                          data-vv-scope="formLienHe"
                                          data-vv-name="Số điện thoại">
                            </v-text-field>
                        </v-flex>
                        <v-flex cols="12" md="12" class="py-0">
                            <v-text-field v-model="lienHe.NoiDung"
                                          :readonly="checkReadOnly"
                                          label="Nội dung"
                                          :counter="200"
                                          v-validate="'required'"
                                          :error-messages="errors.collect('Nội dung', 'formLienHe')"
                                          data-vv-scope="formLienHe"
                                          data-vv-name="Nội dung">
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
    import LienHeApi, { LienHeApiSearchParams } from '@/apiResources/LienHeApi';
    import { LienHe } from '@/models/LienHe';
    import NguoiDungApi, { NguoiDungApiSearchParams } from '@/apiResources/NguoiDungApi';
    import { NguoiDung } from '@/models/NguoiDung';

    export default Vue.extend({
        $_veeValidate: {
            validator: 'new'
        },
        components: {},
        data() {
            return {
                dsNguoiDung: [] as NguoiDung[],
                searchParamsNguoiDung: { includeEntities: true, rowsPerPage: 10 } as NguoiDungApiSearchParams,
                isUpdate: false,
                lienHe: {} as LienHe,
                valid: false,
                isShow: false,
                loading: false,
                loadingSave: false,
                lienHeID: 0,
                searchParamsLienHe: {} as LienHeApiSearchParams,
                checkReadOnly: false,
            }
        },
        watch: {
        },
        created() {
            this.getDanhSachNguoiDung();
        },
        methods: {
             hide() {
                this.isShow = false
            },
            show(isUpdate: boolean, item: any) {
                this.isShow = true;
                this.loadingSave = false;
                this.isUpdate = isUpdate;
                if (isUpdate) {
                    this.checkReadOnly = true;
                    this.lienHeID = item.LienHeID;
                    this.getDataFromApi(this.lienHeID);
                }
                else {
                    this.lienHe = {} as LienHe;
                    this.checkReadOnly = false;
                }
            },
            getDanhSachNguoiDung(): void {
                NguoiDungApi.search(this.searchParamsNguoiDung).then(res => {
                    this.dsNguoiDung = res.Data;
                });
            },
            getDataFromApi(id: number): void {
                LienHeApi.detail(id).then(res => {
                    this.lienHe = res;
                });
            },
            save(): void {
                this.$validator.validateAll('formLienHe').then((res) => {
                   if (res) {
                        this.lienHe.NguoiDung = undefined;
                        if (this.isUpdate) {
                            this.loading = true;
                            this.loadingSave = true;
                            LienHeApi.update(this.lienHeID, this.lienHe).then(res => {
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
                            LienHeApi.insert(this.lienHe).then(res => {
                                this.lienHe = res;
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
            },
        }
    });
</script>

