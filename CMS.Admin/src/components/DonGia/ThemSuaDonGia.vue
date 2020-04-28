<template>
    <v-dialog v-model="isShow" width="600" scrollable persistent>
        <v-card>
            <v-card-title class="teal lighten-2 white--text pa-2">
                <span class="title">{{ isUpdate? 'Cập nhập đơn giá' : 'Thêm mới đơn giá' }}</span>
                <v-spacer></v-spacer>
                <v-btn class="white--text ma-0" small icon @click="hide">
                    <v-icon>close</v-icon>
                </v-btn>
            </v-card-title>
            <v-card-text>
                <v-form ref="form" v-model="valid" scope="formDonGia">
                    <v-layout row wrap>
                        <v-flex xs12 md6>
                            <v-autocomplete v-model="donGia.HinhThucGiatID"
                                      :items="dsHinhThucGiat"
                                      item-text="TenHinhThuc"
                                      item-value="HinhThucGiatID"
                                      label="Hình thức giặt"
                                      :error-messages="errors.collect('Hình thức giặt', 'formDonGia')"
                                      v-validate="'required'"
                                      data-vv-scope="formDonGia"
                                      data-vv-name="Hình thức giặt" class="mr-1 ml-1"
                                      required></v-autocomplete>
                        </v-flex>
                        <v-flex xs12 md6>
                            <v-autocomplete v-model="donGia.DoGiatID"
                                      :items="dsDoGiat"
                                      item-text="TenDoGiat"
                                      item-value="DoGiatID"
                                      label="Tên đồ giặt"
                                      :error-messages="errors.collect('Tên đồ giặt', 'formDonGia')"
                                      v-validate="'required'"
                                      data-vv-scope="formDonGia" class="mr-1 ml-1"
                                      data-vv-name="Tên đồ giặt"
                                      required></v-autocomplete>
                        </v-flex>
                        <v-flex xs12>
                            <v-text-field v-model="donGia.DonGiaGiat"
                                          label="Đơn giá" type="number"
                                          :error-messages="errors.collect('Giá từ', 'formDonGia')"
                                          v-validate="'required|numeric'" class="mr-1 ml-1"
                                          data-vv-scope="formDonGia"
                                          data-vv-name="Giá từ">
                            </v-text-field>
                        </v-flex>
                        <v-flex xs12>
                            <v-text-field v-model="donGia.GhiChu"
                                          label="Ghi chú"
                                          :counter="200"
                                          :error-messages="errors.collect('Ghi chú', 'formDonGia')"
                                          data-vv-scope="formDonGia" class="mr-1 ml-1"
                                          data-vv-name="Ghi chú">
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
    import DonGiaApi, { DonGiaApiSearchParams } from '@/apiResources/DonGiaApi';
    import { DonGia } from '@/models/DonGia';
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
                dsDoGiat: [] as DoGiat[],
                dsHinhThucGiat: [] as HinhThucGiat[],
                isUpdate: false,
                donGia: {} as DonGia,
                valid: false,
                isShow: false,
                loading: false,
                loadingSave: false,
                donGiaID: 0,
                searchParamsDonGia: {} as DonGiaApiSearchParams,
                searchParamsDoGiat: { includeEntities: true, rowsPerPage: 10 } as DoGiatApiSearchParams,
                searchParamsHinhThucGiat: { includeEntities: true, rowsPerPage: 10 } as HinhThucGiatApiSearchParams,
            }
        },
        watch: {
        },
        created() {
            this.getDanhSachDoGiat();
            this.getDanhSachHinhThucGiat();
        },
        methods: {
             hide() {
                this.isShow = false
            },
            show(isUpdate: boolean, item: any) {
                this.isShow = true;
                this.$validator.reset()
                this.loadingSave = false;
                this.isUpdate = isUpdate;
                if (isUpdate) {
                    this.donGiaID = item.DonGiaID;
                    this.getDataFromApi(this.donGiaID);
                }
                else {
                    this.donGia = {} as DonGia;
                }
            },
            getDanhSachDoGiat(): void {
                DoGiatApi.search(this.searchParamsDoGiat).then(res => {
                    this.dsDoGiat = res.Data;
                });
            },
            getDanhSachHinhThucGiat(): void {
                HinhThucGiatApi.search(this.searchParamsHinhThucGiat).then(res => {
                    this.dsHinhThucGiat = res.Data;
                });
            },
            getDataFromApi(id: number): void {
                DonGiaApi.detail(id).then(res => {
                    this.donGia = res;
                });
            },
            save(): void {
                this.$validator.validateAll('formDonGia').then((res) => {
                   if (res) {
                        this.donGia.DoGiat = undefined;
                        this.donGia.HinhThucGiat = undefined;
                        if (this.isUpdate) {
                            this.loading = true;
                            this.loadingSave = true;
                            DonGiaApi.update(this.donGiaID, this.donGia).then(res => {
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
                            DonGiaApi.insert(this.donGia).then(res => {
                                this.donGia = res;
                                this.isUpdate = true;
                                this.loadingSave = false;
                                this.loading = false;
                                this.isShow = false;
                                this.$emit("save");
                                this.$snotify.success('Thêm mới thành công!');
                            }).catch(res => {
                                this.loading = false;
                                this.loadingSave = false;
                                this.$snotify.error('Đơn giá đã tồn tại!');
                            });
                        }
                    }
                });
            },
        }
    });
</script>

