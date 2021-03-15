using System;
using System.Collections.Generic;
using System.Text;

namespace CME.Entities.Constants
{
    public static class Default
    {
        public static string OrganizationId = "00000000-0000-0000-0000-000000000001";
        public static string OrganizationName = "BV Xanh Pôn";
        public static string OrganizationCode = "BVXP";
        public static string OrganizationAddress = "12 Chu Văn An, Điện Bàn, Ba Đình, Hà Nội";


        public static string TrainingFormId_HTHN = "00000000-0000-0000-0000-200000000001";
        public static string TrainingFormName_HTHN = "Hội thảo, hội nghị";
        public static string TrainingFormCode_HTHN = "HTHN";

        public static string TrainingSubjectId_Participant_HTHN = "00000000-0000-0000-0000-300000000001";
        public static string TrainingSubjectName_Participant_HTHN = "Người tham dự";
        public static int TrainingSubjectAmount_Participant_HTHN = 4;

        public static string TrainingSubjectId_Owner_HTHN = "00000000-0000-0000-0000-300000000002";
        public static string TrainingSubjectName_Owner_HTHN = "Người thuyết trình";
        public static int TrainingSubjectAmount_Owner_HTHN = 8;


        public static string TrainingFormId_NCKH = "00000000-0000-0000-0000-200000000002";
        public static string TrainingFormName_NCKH = "Nghiên cứu khoa học";
        public static string TrainingFormCode_NCKH = "NCKH";

        public static string TrainingFormId_DTDH = "00000000-0000-0000-0000-200000000003";
        public static string TrainingFormName_DTDH = "Đào tạo dài hạn";
        public static string TrainingFormCode_DTDH = "DTDH";

        public static string TrainingSubjectId_Participant_DTDH = "00000000-0000-0000-0000-330000000001";
        public static string TrainingSubjectName_Participant_DTDH = "NV nội vụ";
        public static int TrainingSubjectAmount_Participant_DTDH = 24;

        public static string Password = "123456a@";

        public static string RoleId_Admin = "00000000-2222-0000-0000-000000000001";
        public static string RoleName_Admin = "Admin";
        public static string RoleCode_Admin = "admin";

        public static string RoleId_User = "00000000-2222-0000-0000-000000000002";
        public static string RoleName_User = "Người dùng";
        public static string RoleCode_User = "normal-user";
    }
}
