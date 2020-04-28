<template>
    <v-dialog v-model="isShow" width="600" scrollable persistent>
        <v-card>
            <v-card-title class="teal lighten-2 white--text pa-2">
                <span class="title">{{ isUpdate? 'Cập nhập hình thức giặt' : 'Thêm mới hình thức giặt' }}</span>
                <v-spacer></v-spacer>
                <v-btn class="white--text ma-0" small icon @click="hide">
                    <v-icon>close</v-icon>
                </v-btn>
            </v-card-title>
            <v-card-text>
                <v-form ref="form" v-model="valid" scope="formHinhThucGiat">
                    <v-layout row wrap>
                        <v-flex xs12>
                            <v-text-field v-model="hinhThucGiat.TenHinhThuc"
                                          label="Tên hình thức giặt"
                                          :counter="50"
                                          :error-messages="errors.collect('Tên hình thức giặt', 'formHinhThucGiat')"
                                          v-validate="'required|max:50'"
                                          data-vv-scope="formHinhThucGiat"
                                          data-vv-name="Tên hình thức giặt">
                            </v-text-field>
                        </v-flex>
                        <v-flex xs12>
                            <v-text-field v-model="hinhThucGiat.GhiChu"
                                          label="Ghi chú"
                                          :error-messages="errors.collect('Ghi chú', 'formHinhThucGiat')"
                                          v-validate="''"
                                          data-vv-scope="formHinhThucGiat"
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
    import HinhThucGiatApi, { HinhThucGiatApiSearchParams } from '@/apiResources/HinhThucGiatApi';
    import { HinhThucGiat } from '@/models/HinhThucGiat';

    export default Vue.extend({
        data() {
            return {
                isUpdate: false,
                valid: false,
                isShow: false,
                hinhThucGiat: {} as HinhThucGiat,
                loading: false,
                loadingSave: false,
                hinhThucGiatID: 0,
                searchParamsHinhThucGiat: {} as HinhThucGiatApiSearchParams,
            };
        },
        watch: {
            isShow() {
                this.$validator.errors.clear();
            }
        },
        $_veeValidate: {
            validator: 'new'
        },
        created() {
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
                    this.hinhThucGiatID = item.HinhThucGiatID;
                    this.getDataFromApi(this.hinhThucGiatID);
                }
                else {
                    this.hinhThucGiat = {} as HinhThucGiat;
                }
            },
            getDataFromApi(id: number): void {
                HinhThucGiatApi.detail(id).then(res => {
                    this.hinhThucGiat = res;
                });
            },
            save(): void {
                this.$validator.validateAll('formHinhThucGiat').then((res) => {
                    if (res) {
                        if (this.isUpdate) {
                            this.loading = true;
                            this.loadingSave = true;
                            HinhThucGiatApi.update(this.hinhThucGiatID, this.hinhThucGiat).then(res => {
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
                            HinhThucGiatApi.insert(this.hinhThucGiat).then(res => {
                                this.hinhThucGiat = res;
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
