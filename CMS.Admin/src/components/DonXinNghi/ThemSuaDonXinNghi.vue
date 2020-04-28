<template>
    <v-flex xs12>
        <v-breadcrumbs divider="/" class="pa-0">
            <v-icon slot="divider">chevron_right</v-icon>
            <v-breadcrumbs-item>
                <v-btn flat class="ma-0" @click="$router.go(-1)" small><v-icon>arrow_back</v-icon> Quay lại</v-btn>
            </v-breadcrumbs-item>
            <v-breadcrumbs-item to="/danhmucsanpham" exact>Danh mục sản phẩm</v-breadcrumbs-item>
            <v-breadcrumbs-item>{{isUpdate?'Cập nhật':'Thêm mới'}}</v-breadcrumbs-item>
        </v-breadcrumbs>
        <v-card>
            <v-card-text>
                <v-form scope="frmAddEdit">
                    <v-layout row wrap>
                        <v-flex xs6 sm3 md3>
                            <v-datetimepicker v-model="leave_Request.TimeStart"
                                              label="Thời gian ra"
                                              :error-messages="errors.collect('Thời gian ra', ' frmAddEdit')"
                                              v-validate="'required'"
                                              data-vv-scope="frmAddEdit"
                                              data-vv-name="Thời gian vào"
                                              hide-details
                                              clearable></v-datetimepicker>
                        </v-flex>
                        <v-flex xs6 sm3 md3>
                            <v-datetimepicker v-model="leave_Request.TimeEnd"
                                              label="Thời gian vào"
                                              :error-messages="errors.collect('Thời gian vào', ' frmAddEdit')"
                                              v-validate="'required'"
                                              data-vv-scope="frmAddEdit"
                                              data-vv-name="Thời gian vào"
                                              hide-details
                                              clearable></v-datetimepicker>
                        </v-flex>
                        <v-flex xs12>
                            <v-textarea v-model="leave_Request.Reason"
                                        counter="500"
                                        auto-grow
                                        label="Lý do"
                                        type="text"
                                        :error-messages="errors.collect('Lý do', 'frmAddEdit')"
                                        v-validate="''"
                                        data-vv-scope="frmAddEdit"
                                        clearable
                                        data-vv-name="Lý do"><Lý dov-textarea>
                        </v-flex>

                    </v-layout>
                </v-form>
            </v-card-text>
            <v-card-actions>
                <v-spacer></v-spacer>
                <v-btn class="primary" :disabled="loading" :loading="loading" @click.native="save">{{isUpdate?'Cập nhật':'Thêm mới'}}</v-btn>
            </v-card-actions>
        </v-card>
    </v-flex>
</template>

<script lang="ts">
    import { Vue } from 'vue-property-decorator';
    import VChooseFile from '@/components/Commons/VChooseFile.vue';
    import Leave_RequestApi, { Leave_RequestApiSearchParams } from '@/apiResources/Leave_RequestApi';
    import { Leave_Request } from '@/models/Leave_Request';
    import { Users } from '@/models/Users';
import UsersApi from '../../apiResources/UsersApi';
    export default Vue.extend({
        $_veeValidate: {
            validator: 'new'
        },
        components: {VChooseFile},
        data() {
            return {
                isUpdate: false,
                dsNhanVien: [] as Users[],
                leave_Request: {} as Leave_Request,
                loading: false,
                searchParamsLeave_Request: {} as Leave_RequestApiSearchParams,
                editing: null as any,
                search: null,
                index: -1,
            }
        },
        watch: {
        },
        mounted() {
            this.getDSNhanVien();
        },
        computed: {
        },
        methods: {
            getDataFromApi(id: number): void {
                Leave_RequestApi.detail(id).then(res => {
                    this.leave_Request = res;
                });
            },
            getDSNhanVien(): void {
                UsersApi.search({ rowsPerPage: -1 }).then(res => {
                    this.dsNhanVien = res.Data;
                });
            },
            save(): void {
                this.$validator.validateAll('frmAddEdit').then((res) => {
                    if (res) {
                        this.leave_Request.User_Approve = undefined;
                        this.leave_Request.User_Leave = undefined;
                        this.leave_Request.Id = this.$store.state.user.Profile.UserId
                        if (this.isUpdate) {
                            let id = parseInt(this.$route.params.id, 10);
                            this.loading = true;
                            Leave_RequestApi.update(id, this.leave_Request).then(res => {
                                this.loading = false;
                                this.$snotify.success('Cập nhật thành công!');
                            }).catch(res => {
                                this.loading = false;
                                this.$snotify.error('Cập nhật thất bại!');
                            });
                        } else {
                            this.loading = true;
                            Leave_RequestApi.insert(this.leave_Request).then(res => {
                                this.leave_Request = res;
                                this.isUpdate = true;
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

