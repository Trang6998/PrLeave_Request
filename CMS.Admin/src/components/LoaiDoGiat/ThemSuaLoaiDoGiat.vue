<template>
    <v-dialog v-model="isShow" width="600" scrollable persistent>
        <v-card>
            <v-card-title class="teal lighten-2 white--text pa-2">
                <span class="title">{{ isUpdate? 'Cập nhập loại đồ giặt' : 'Thêm mới loại đồ giặt' }}</span>
                <v-spacer></v-spacer>
                <v-btn class="white--text ma-0" small icon @click="hide">
                    <v-icon>close</v-icon>
                </v-btn>
            </v-card-title>
            <v-card-text>
                <v-form ref="form" v-model="valid" scope="formLoaiDoGiat">
                    <v-layout row wrap>
                        <v-flex xs12>
                            <v-text-field v-model="loaiDoGiat.TenLoaiDoGiat"
                                          label="Tên loại đồ giặt"
                                          :counter="50"
                                          :error-messages="errors.collect('Tên loại đồ giặt', 'formLoaiDoGiat')"
                                          v-validate="'required|max:50'"
                                          data-vv-scope="formLoaiDoGiat"
                                          data-vv-name="Tên loại đồ giặt">
                            </v-text-field>
                        </v-flex>
                        <v-flex xs12>
                            <v-text-field v-model="loaiDoGiat.MoTa"
                                          label="Mô tả"
                                          :counter="200"
                                          :error-messages="errors.collect('Mô tả', 'formLoaiDoGiat')"
                                          data-vv-scope="formLoaiDoGiat"
                                          data-vv-name="Mô tả">
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
    import LoaiDoGiatApi, { LoaiDoGiatApiSearchParams } from '@/apiResources/LoaiDoGiatApi';
    import { LoaiDoGiat } from '@/models/LoaiDoGiat';

    export default Vue.extend({
        $_veeValidate: {
            validator: 'new'
        },
        components: {},
        data() {
            return {
                isUpdate: false,
                valid: false,
                loaiDoGiat: {} as LoaiDoGiat,
                isShow: false,
                loading: false,
                loadingSave: false,
                loaiDoGiatID: 0,
                searchParamsLoaiDoGiat: {} as LoaiDoGiatApiSearchParams,
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
                this.loadingSave = false;
                this.isUpdate = isUpdate;
                if (isUpdate) {
                    this.loaiDoGiatID = item.LoaiDoGiatID;
                    this.getDataFromApi(this.loaiDoGiatID);
                }
                else {
                    this.loaiDoGiat = {} as LoaiDoGiat;
                }
            },
            getDataFromApi(id: number): void {
                LoaiDoGiatApi.detail(id).then(res => {
                    this.loaiDoGiat = res;
                });
            },
            save(): void {
                this.$validator.validateAll('formLoaiDoGiat').then((res) => {
                    if (res) {
                        if (this.isUpdate) {
                            this.loading = true;
                            this.loadingSave = true;
                            LoaiDoGiatApi.update(this.loaiDoGiatID, this.loaiDoGiat).then(res => {
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
                            LoaiDoGiatApi.insert(this.loaiDoGiat).then(res => {
                                this.loaiDoGiat = res;
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

