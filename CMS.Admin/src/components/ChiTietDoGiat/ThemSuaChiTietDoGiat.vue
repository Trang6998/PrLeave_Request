<!--<template>
    <v-dialog v-model="isShow" width="600" scrollable persistent>
        <v-card>
            <v-card-title class="teal lighten-2 white--text pa-2">
                <span class="title">{{ isUpdate? 'Cập nhập chi tiết đơn' : 'Thêm mới chi tiết đơn' }}</span>
                <v-spacer></v-spacer>
                <v-btn class="white--text ma-0" small icon @click="hide">
                    <v-icon>close</v-icon>
                </v-btn>
            </v-card-title>
            <v-card-text>
                <v-form ref="form" v-model="valid" scope="formChiTietDoGiat">
                    <v-row>
                        <v-col cols="12" md="12" class="py-0">
                            <v-autocomplete v-model="chiTietDoGiat.DoGiatID"
                                            :items="dsDoGiat"
                                            item-text="TenDoGiat"
                                            item-value="DoGiatID"
                                            label="Tên đồ giặt"
                                            :error-messages="errors.collect('Tên đồ giặt', 'formChiTietDoGiat')"
                                            v-validate="'required'"
                                            data-vv-scope="formChiTietDoGiat"
                                            data-vv-name="Tên đồ giặt"
                                            required></v-autocomplete>
                        </v-col>
                        <v-col cols="12" md="12" class="py-0">
                            <v-autocomplete v-model="chiTietDoGiat.HinhThucGiatID"
                                            :items="dsHinhThucGiat"
                                            item-text="TenHinhThuc"
                                            item-value="HinhThucGiatID"
                                            label="Hình thức giặt"
                                            :error-messages="errors.collect('Hình thức giặt', 'formChiTietDoGiat')"
                                            v-validate="'required'"
                                            data-vv-scope="formChiTietDoGiat"
                                            data-vv-name="Hình thức giặt"
                                            required></v-autocomplete>
                        </v-col>
                        <v-col cols="12" md="12" class="py-0">
                            <v-text-field v-model="chiTietDoGiat.SoLuong"
                                          label="Số lượng"
                                          :counter="50"
                                          :error-messages="errors.collect('Số lượng', 'formChiTietDoGiat')"
                                          v-validate="'numeric'"
                                          data-vv-scope="formChiTietDoGiat"
                                          data-vv-name="Số lượng">
                            </v-text-field>
                        </v-col>
                    </v-row>
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
    import ChiTietDoGiatApi, { ChiTietDoGiatApiSearchParams } from '@/apiResources/ChiTietDoGiatApi';
    import { ChiTietDoGiat } from '@/models/ChiTietDoGiat';
    import DoGiatApi, { DoGiatApiSearchParams } from '@/apiResources/DoGiatApi';
    import { DoGiat } from '@/models/DoGiat';
    import HinhThucGiatApi, { HinhThucGiatApiSearchParams } from '@/apiResources/HinhThucGiatApi';
    import { HinhThucGiat } from '@/models/HinhThucGiat';

    export default Vue.extend({
        $_veeValidate: {
            validator: 'new'
        },
        components: {},
        data() {
            return {
                dsHinhThucGiat: [] as HinhThucGiat[],
                dsDoGiat: [] as DoGiat[],
                isUpdate: false,
                chiTietDoGiat: {} as ChiTietDoGiat,
                loading: false,
                valid: false,
                isShow: false,
                loadingSave: false,
                chiTietDoGiatID: 0,
                searchParamsChiTietDoGiat: {} as ChiTietDoGiatApiSearchParams,
                searchParamsHinhThucGiat: { includeEntities: true, rowsPerPage: 10 } as HinhThucGiatApiSearchParams,
                searchParamsDoGiat: { includeEntities: true, rowsPerPage: 10 } as DoGiatApiSearchParams,
            }
        },
        watch: {
        },
        created() {
            this.getDanhSachHinhThucGiat();
            this.getDanhSachDoGiat();
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
                    this.chiTietDoGiatID = item.ChiTietDoGiatID;
                    this.getDataFromApi(this.chiTietDoGiatID);
                }
                else {
                    this.chiTietDoGiat = {} as ChiTietDoGiat;
                }
            },
            getDanhSachHinhThucGiat(): void {
                HinhThucGiatApi.search(this.searchParamsHinhThucGiat).then(res => {
                    this.dsHinhThucGiat = res.Data;
                });
            },
            getDanhSachDoGiat(): void {
                DoGiatApi.search(this.searchParamsDoGiat).then(res => {
                    this.dsDoGiat = res.Data;
                });
            },
            getDataFromApi(id: number): void {
                ChiTietDoGiatApi.detail(id).then(res => {
                    this.chiTietDoGiat = res;
                });
            },
            save(): void {
                this.$validator.validateAll('formChiTietDoGiat').then((res) => {
                    if (res) {
                        this.chiTietDoGiat.DoGiat = undefined;
                        this.chiTietDoGiat.HinhThucGiat = undefined;
                        if (this.isUpdate) {
                            this.loading = true;
                            this.loadingSave = true;
                            ChiTietDoGiatApi.update(this.chiTietDoGiatID, this.chiTietDoGiat).then(res => {
                                this.loading = false;
                                this.$snotify.success('Cập nhật thành công!');
                            }).catch(res => {
                                this.loading = false;
                                this.isShow = false;
                                this.loadingSave = false;
                                this.$emit("save");
                                this.$snotify.error('Cập nhật thất bại!');
                            });
                        } else {
                            this.loading = true;
                            this.loadingSave = true;
                            ChiTietDoGiatApi.insert(this.chiTietDoGiat).then(res => {
                                this.chiTietDoGiat = res;
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
</script>-->

